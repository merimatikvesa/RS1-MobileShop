using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }
        public string Name { get; set; }
    }
}
