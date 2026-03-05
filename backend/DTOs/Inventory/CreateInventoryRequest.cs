namespace backend.DTOs.Inventory
{
    public class CreateInventoryRequest
    {
            public int ProductId { get; set; }
            public int QuantityInStock { get; set; }
    }
}
