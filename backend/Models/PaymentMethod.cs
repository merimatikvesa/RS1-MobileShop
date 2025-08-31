using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }
        public string Type { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
