using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required]
        public string SupplierName { get; set;}
        [Required]
        public string PhoneNumber { get; set;}
        [Required]
        public string Address { get; set;}

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
