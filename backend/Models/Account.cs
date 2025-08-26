using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
