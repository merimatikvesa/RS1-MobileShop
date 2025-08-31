using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Inventory
    {
        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}