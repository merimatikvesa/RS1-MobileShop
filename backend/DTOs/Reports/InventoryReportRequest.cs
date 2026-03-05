namespace backend.DTOs.Reports
{
    public class InventoryReportRequest
    {
        public int? ProductId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
