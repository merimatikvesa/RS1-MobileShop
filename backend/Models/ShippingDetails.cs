using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class ShippingDetails
    {
        [Key]
        public int ShippingId { get; set; }
        [Required]   
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        public DateTime ShippingDate { get; set; } = DateTime.Now;
        [Required]
        public int ShipmentMethodId { get; set; }
        [ForeignKey("ShipmentMethodId")]
        public ShipmentMethod ShipmentMethod { get; set; }


    }
}
