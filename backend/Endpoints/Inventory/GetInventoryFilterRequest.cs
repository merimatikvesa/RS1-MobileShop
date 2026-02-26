using backend.Models;

namespace backend.Endpoints.Inventories
{
    public class GetInventoryFilterRequest
    {
        public int? ProductId { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public string? ProductName { get; set; }
        public bool? IsLowStock { get; set; }
    }
}
