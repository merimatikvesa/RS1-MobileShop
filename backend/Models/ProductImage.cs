using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
