namespace backend.DTOs.Products
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public int? PromotionId { get; set; }
        public string ImagePath { get; set; } = "";
    }
}
