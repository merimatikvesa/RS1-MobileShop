using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }
    }
}
