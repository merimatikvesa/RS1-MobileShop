using backend.DTOs.Auth;
using backend.Helper;           
using backend.Models;
using backend.Services.Auth;     
using backend.Data;             
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace backend.Endpoints.Auth
{
    [AllowAnonymous]
    public class LoginEndpoint
        : MyEndpointBaseAsync.WithRequest<LoginRequestDto, LoginResponseDto>
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwt;

        public LoginEndpoint(MyDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("api/auth/login")]
        public override async Task<ActionResult<LoginResponseDto>> HandleAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            // Basic validation (with DataAnnotations on DTO)
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password)) // TODO: Add password hashing in future implementation
            {
                return BadRequest(new { errors = new[] { "Username and password are required." } });
            }

            // NOTE: For demo comparison of plain-password. 
            // In production: hash + salt, then comparison of hashes.
            var account = await _db.Accounts
                .FirstOrDefaultAsync(a =>
                    a.Username == request.Username && a.Password == request.Password,
                    cancellationToken);

            if (account == null)
                return Unauthorized(new { errors = new[] { "Invalid username or password." } });

            // Access Jwt token
            var (token, expiresInMinutes) = _jwt.GenerateToken(account);
            // Refresh Jwt token 
            var refreshTokenValue = _jwt.GenerateRefreshToken();
            var refreshEntity = new RefreshToken
            {
                Token = refreshTokenValue,
                AccountId = account.AccountId,          
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _db.RefreshTokens.Add(refreshEntity);
            await _db.SaveChangesAsync(cancellationToken);

          

            var response = new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshTokenValue,
                Username = account.Username,
                ExpiresInMinutes = expiresInMinutes
            };

            return Ok(response);
        }
    }
}
