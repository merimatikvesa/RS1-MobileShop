using Ardalis.ApiEndpoints;
using backend.Data;
using backend.DTOs.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace backend.Endpoints.Reports
{
    public class InventoryReportEndpoint
        : EndpointBaseAsync
            .WithRequest<InventoryReportRequest>
            .WithActionResult
    {
        private readonly MyDbContext _db;

        public InventoryReportEndpoint(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("api/reports/inventory")]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] InventoryReportRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = _db.Inventory
                .AsNoTracking()
                .Include(x => x.Product)
                .AsQueryable();

            if (request.ProductId.HasValue)
                query = query.Where(x => x.ProductId == request.ProductId.Value);

            query = query.Where(x =>
                x.LastUpdated >= request.StartDate &&
                x.LastUpdated <= request.EndDate);

            var rows = await query
                .Select(x => new InventoryRow(
                    x.ProductId,
                    x.Product.ProductName,
                    x.QuantityInStock,
                    x.LastUpdated
                ))
                .ToListAsync(cancellationToken);

            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = BuildPdf(rows);

            return File(pdfBytes, "application/pdf", $"inventory_report_{DateTime.UtcNow:yyyyMMddHHmm}.pdf");
        }

        private static byte[] BuildPdf(List<InventoryRow> rows)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);

                    page.Header().Text("Inventory Report").FontSize(18).Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(80);
                            c.RelativeColumn();
                            c.ConstantColumn(80);
                            c.ConstantColumn(120);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Text("Product ID");
                            h.Cell().Text("Product");
                            h.Cell().Text("Stock");
                            h.Cell().Text("Updated");
                        });

                        foreach (var r in rows)
                        {
                            table.Cell().Text(r.ProductId.ToString());
                            table.Cell().Text(r.ProductName);
                            table.Cell().Text(r.QuantityInStock.ToString());
                            table.Cell().Text(r.LastUpdated.ToString("yyyy-MM-dd"));
                        }
                    });
                });
            }).GeneratePdf();
        }

        public record InventoryRow(
            int ProductId,
            string ProductName,
            int QuantityInStock,
            DateTime LastUpdated
        );
    }
}