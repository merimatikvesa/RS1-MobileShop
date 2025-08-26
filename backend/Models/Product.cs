using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set;}    
        public int PromotionId { get; set; }

    }
}
