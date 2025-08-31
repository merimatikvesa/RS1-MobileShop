using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }    
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
