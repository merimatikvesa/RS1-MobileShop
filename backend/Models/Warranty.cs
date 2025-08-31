using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
 
    public class Warranty
    {
        [Key]
        public int WarrantyId { get; set; }
        public string Type { get; set; }
        public DateTime Duration { get; set; }

        public ICollection<Phone> Phones { get; set; } = new List<Phone>();

    }
}
