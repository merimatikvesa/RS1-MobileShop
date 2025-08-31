using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public ICollection<ProductImage> ProductImages { get; set;} = new List<ProductImage>();
    }
}
