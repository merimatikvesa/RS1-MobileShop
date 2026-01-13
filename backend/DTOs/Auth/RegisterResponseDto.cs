namespace backend.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
