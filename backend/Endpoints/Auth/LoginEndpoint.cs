using backend.DTOs.Auth;
using backend.Helper;           
using backend.Models;
using backend.Services.Auth;     
using backend.Data;             
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Services.Security;

namespace backend.Endpoints.Auth
{
    [AllowAnonymous]
    public class LoginEndpoint
        : MyEndpointBaseAsync.WithRequest<LoginRequestDto, LoginResponseDto>
    {
        private readonly MyDbContext _db;
        private readonly IJwtService _jwt;
        private readonly RecaptchaService _recaptcha;

        public LoginEndpoint(MyDbContext db, IJwtService jwt, RecaptchaService recaptcha)
        {
            _db = db;
            _jwt = jwt;
            _recaptcha = recaptcha;
        }


        [HttpPost("api/auth/login")]
        public override async Task<ActionResult<LoginResponseDto>> HandleAsync(
            LoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            // Basic validation (with DataAnnotations on DTO)
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password)) 
            {
                return BadRequest(new { errors = new[] { "Username and password are required." } });
            }

            var captchaOk = await _recaptcha.VerifyAsync(request.RecaptchaToken, cancellationToken);
            if (!captchaOk)
                return BadRequest(new { errors = new[] { "Captcha verification failed." } });

            var account = await _db.Accounts
                .FirstOrDefaultAsync(a =>
                    a.Username == request.Username && a.Password == request.Password,
                    cancellationToken);

            if (account == null)
                return Unauthorized(new { errors = new[] { "Invalid username or password." } });
            
            var isAdmin = await _db.Administrators
            .AnyAsync(a => a.AccountId == account.AccountId, cancellationToken);

            var role = isAdmin ? "Admin" : "User";

            // Access Jwt token
            var (token, expiresInMinutes) = _jwt.GenerateToken(account, role);
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
                ExpiresInMinutes = expiresInMinutes,
                Role = role
            };

            return Ok(response);
        }
    }
}
