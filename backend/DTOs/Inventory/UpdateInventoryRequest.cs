namespace backend.DTOs.Inventory
{
    public class UpdateInventoryRequest
    {
            public int ProductId { get; set; }
            public int QuantityInStock { get; set; }
        
    }
}
