namespace backend.DTOs.Reports
{
    public class ProductsReportRequest
    {
        public int? Id { get; set; }             
        public string? ProductName { get; set; }  
        public DateTime StartDate { get; set; }     
        public DateTime EndDate { get; set; }
    }
}
