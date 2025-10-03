using Azure.Core;
using backend.Data;
using backend.DTOs.Auth;
using backend.Helper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace backend.Endpoints.Auth
{
    [Authorize] // logout only autent. account
    public class LogoutEndpoint
          : MyEndpointBaseAsync.WithRequest<LogoutRequestDto, object>
    {
        private readonly MyDbContext _db;

        public LogoutEndpoint(MyDbContext db)
        {
            _db = db;
        }

        [HttpPost("api/auth/logout")]
        public override async Task<ActionResult<object>> HandleAsync(
            LogoutRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(sub) || !int.TryParse(sub, out var accountId))
                return Unauthorized(new { errors = new[] { "Invalid user context." } });

            if (request.AllSessions)
            {
                var activeTokens = await _db.RefreshTokens
                    .Where(rt => rt.AccountId == accountId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
                    .ToListAsync(cancellationToken);

                foreach (var rt in activeTokens) rt.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) ||
                    string.IsNullOrWhiteSpace(refreshToken))
                    return Ok(new { message = "Already logged out." });

                var rt = await _db.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == refreshToken && x.RevokedAt == null, cancellationToken);

                if (rt != null && rt.AccountId == accountId)
                {
                    rt.RevokedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }

            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok(new { message = request.AllSessions ? "Logged out from all sessions." : "Logged out." });

        }
    }
}
