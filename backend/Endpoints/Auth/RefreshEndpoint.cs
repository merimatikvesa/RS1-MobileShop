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

        [HttpPost("api/auth/refresh")]
        public override async Task<ActionResult<LoginResponseDto>> HandleAsync(
            RefreshRequestDto request,
            CancellationToken cancellationToken = default)
        {
            // Resolve services manually because EndpointBaseAsync does not support constructor DI
            var _db = HttpContext.RequestServices.GetRequiredService<MyDbContext>();
            var _jwt = HttpContext.RequestServices.GetRequiredService<IJwtService>();

            var refreshToken = request.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Unauthorized(new { errors = new[] { "Missing refresh token." } });

            var refreshEntity = await _db.RefreshTokens
                .Include(r => r.Account)
                .FirstOrDefaultAsync(
                    r => r.Token == refreshToken && r.RevokedAt == null,
                    cancellationToken);

            if (refreshEntity == null || refreshEntity.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized(new { errors = new[] { "Invalid or expired refresh token." } });


            var isAdmin = await _db.Administrators
            .AnyAsync(a => a.AccountId == refreshEntity.Account.AccountId, cancellationToken);

            var role = isAdmin ? "Admin" : "User";

            var (newAccess, expiresInMinutes) = _jwt.GenerateToken(refreshEntity.Account, role);


            refreshEntity.RevokedAt = DateTime.UtcNow;
            _db.RefreshTokens.Update(refreshEntity);

            var newRefreshValue = _jwt.GenerateRefreshToken();
            _db.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshValue,
                AccountId = refreshEntity.AccountId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync(cancellationToken);

            return Ok(new LoginResponseDto
            {
                Token = newAccess,
                RefreshToken = newRefreshValue,
                Username = refreshEntity.Account.Username,
                ExpiresInMinutes = expiresInMinutes
            });
        }
    }
}
