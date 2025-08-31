using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public ICollection<Favorites> Favorites { get; set; } = new List<Favorites>();
        public ICollection<Order> Orders { get; set; }= new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();



    }
}
