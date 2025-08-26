using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public float Rating {  get; set; }
        public string Comment { get; set; }
    }
}
