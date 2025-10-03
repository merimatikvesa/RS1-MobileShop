using backend.Data;
using backend.DTOs.Auth;
using backend.Helper;
using backend.Models;
using backend.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Auth
{
    public class RefreshEndpoint
    : MyEndpointBaseAsync.WithRequest<RefreshRequestDto, LoginResponseDto>
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwt;

        public RefreshEndpoint(MyDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("api/auth/refresh")]
        public override async Task<ActionResult<LoginResponseDto>> HandleAsync(
            RefreshRequestDto request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { errors = new[] { "Refresh token is required." } });

            var refreshEntity = await _db.RefreshTokens
                .Include(r => r.Account)
                .FirstOrDefaultAsync(
                    r => r.Token == request.RefreshToken && r.RevokedAt == null,
                    cancellationToken);

            if (refreshEntity == null || refreshEntity.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized(new { errors = new[] { "Invalid or expired refresh token." } });

           
            var (newAccess, expiresInMinutes) = _jwt.GenerateToken(refreshEntity.Account);

            refreshEntity.RevokedAt = DateTime.UtcNow;

            var newRefreshValue = _jwt.GenerateRefreshToken();
            var newRefreshEntity = new RefreshToken
            {
                Token = newRefreshValue,
                AccountId = refreshEntity.AccountId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _db.RefreshTokens.Add(newRefreshEntity);
            await _db.SaveChangesAsync(cancellationToken);

            return Ok(new LoginResponseDto
            {
                Token = newAccess,
                Username = refreshEntity.Account.Username,
                ExpiresInMinutes = expiresInMinutes,
                RefreshToken = newRefreshValue
            });
        }
    }
}
