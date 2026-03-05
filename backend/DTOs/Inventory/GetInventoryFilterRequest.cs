using backend.Models;

namespace backend.DTOs.Inventory
{
    public class GetInventoryFilterRequest
    {
        public int? ProductId { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public string? ProductName { get; set; }
        public bool? IsLowStock { get; set; }

        // paging
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
