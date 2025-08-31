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
        public PaymentMethod PaymentMethod { get; set; }    
        public int UserId { get; set; }
        public User User { get; set; }

        public ShippingDetails ShippingDetail { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }= new List<OrderItem>();

    }
}
