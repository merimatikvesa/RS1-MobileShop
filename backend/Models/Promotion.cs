using System.ComponentModel.DataAnnotations;
namespace backend.Models
{
    public class Promotion
    {
        [Key]
        public int PromotionId { get; set; }
        public string PromotionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
