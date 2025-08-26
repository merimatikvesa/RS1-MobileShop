using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
