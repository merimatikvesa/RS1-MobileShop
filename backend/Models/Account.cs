using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public User? User { get; set; }
        public Administrator? Administrator { get; set; }
    }
}
