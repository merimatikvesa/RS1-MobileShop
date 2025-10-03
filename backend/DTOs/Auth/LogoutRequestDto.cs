namespace backend.DTOs.Auth
{
    public class LogoutRequestDto
    {
        // If true - delete all refresh tokens (all devices)
        public bool AllSessions { get; set; } = false;
    }
}
