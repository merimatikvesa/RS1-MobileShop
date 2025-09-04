using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Helper.Auth;
using backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;

        public JwtService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public (string token, int expiresInMinutes) GenerateToken(Account account)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_options.Key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, account.AccountId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, account.Username),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
            };

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpireMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, _options.ExpireMinutes);
        }
    }
}
