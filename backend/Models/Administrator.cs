using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Administrator
    {
        [Key]
        public int AdministratorId { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
