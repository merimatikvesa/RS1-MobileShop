using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ShippingDetails
    {
        [Key]
        public int ShippingId { get; set; }
        public int OrderId { get; set; }
        public string ShippingAdress { get; set; }
        public DateTime ShippingDate { get; set; }
        public int ShipmentMethodId { get; set; }
    }
}
