using backend.Helper.Auth;
using backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;
        private readonly string _signingKey;
        public JwtService(IOptions<JwtOptions> options, IConfiguration configuration)
        {
            _options = options.Value;

            _signingKey = configuration["Jwt:Key"]
                          ?? throw new InvalidOperationException("JWT Key not found in configuration.");
            Console.WriteLine(">>> SIGNING KEY = " + _signingKey);
            if (_signingKey.Length < 16)
                throw new InvalidOperationException("JWT Key too short — must be at least 16 characters.");
        }

        public (string token, int expiresInMinutes) GenerateToken(Account account)
        {

            var keyBytes = Encoding.UTF8.GetBytes(_signingKey);
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
        public string GenerateRefreshToken()
        {
            
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
