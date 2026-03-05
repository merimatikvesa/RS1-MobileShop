namespace backend.DTOs.Products
{
    public class GetProductsFilterRequest
    {
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // paging
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
