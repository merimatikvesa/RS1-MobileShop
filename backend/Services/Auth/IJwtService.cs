using backend.Models;

namespace backend.Services.Auth
{
    public interface IJwtService
    {
        (string token, int expiresInMinutes) GenerateToken(Account account, string role);
        string GenerateRefreshToken();
    }
}
