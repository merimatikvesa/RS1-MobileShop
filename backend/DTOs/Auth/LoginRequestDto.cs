using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string RecaptchaToken { get; set; } = string.Empty;
    }
}
