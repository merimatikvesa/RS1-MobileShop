using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Range(1,5)]
        public float Rating {  get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
