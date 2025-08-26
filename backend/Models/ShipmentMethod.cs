using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace backend.Models
{
    public class ShipmentMethod
    {
        [Key]
        public int ShipmentMethodId { get; set; }
        public string ShipmentCompany {  get; set; }
        public decimal ShipmentPrice { get; set; }
    }
}
