using backend.Data;
using backend.DTOs.Auth;
using backend.Helper;
using backend.Models;
using backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Auth
{
    [AllowAnonymous]
    public class RegisterEndpoint
        : MyEndpointBaseAsync.WithRequest<RegisterRequestDto, RegisterResponseDto>
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwt;

        public RegisterEndpoint(MyDbContext db, IJwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("api/auth/register")]
        public override async Task<ActionResult<RegisterResponseDto>> HandleAsync(
            RegisterRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new
                {
                    errors = new[] { "All fields are required." }
                });
            }

            // Check if user already exists
            var accountExists = await _db.Accounts
                .AnyAsync(a => a.Username == dto.Email, cancellationToken);

            if (accountExists)
                return BadRequest(new
                {
                    errors = new[] { "User already exists." }
                });

            // Split full name
            var firstName = dto.FullName;
            var lastName = "-";

            if (dto.FullName.Contains(" "))
            {
                var parts = dto.FullName.Split(' ', 2);
                firstName = parts[0];
                lastName = parts[1];
            }

            // Create Account
            var account = new Account
            {
                Username = dto.Email,
                Password = dto.Password, // TODO: hash later
                FirstName = firstName,
                LastName = lastName
            };

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync(cancellationToken);

            // Create User
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                AccountId = account.AccountId
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            // Generate JWT + Refresh Token
            var role = "User";
            var (token, expiresInMinutes) = _jwt.GenerateToken(account, role);

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

            // Response (IDENTICAL STYLE TO LOGIN)
            return new RegisterResponseDto
            {
                Token = token,
                RefreshToken = refreshTokenValue,
                Username = account.Username,
                ExpiresInMinutes = expiresInMinutes
            };
        }
    }
}
