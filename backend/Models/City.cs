using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
