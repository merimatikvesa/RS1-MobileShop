using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class OS
    {
        [Key]
        public int OSId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public ICollection<Phone> Phones { get; set; } = new List<Phone>();


    }
}
