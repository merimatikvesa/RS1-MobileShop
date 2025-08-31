using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace backend.Models
{
    public class ShipmentMethod
    {
        [Key]
        public int ShipmentMethodId { get; set; }
        [Required]
        public string ShipmentCompany {  get; set; }
        [Required]
        public decimal ShipmentPrice { get; set; }

        public ICollection<ShippingDetails> ShippingDetails { get; set; } = new List<ShippingDetails>();
    }
}
