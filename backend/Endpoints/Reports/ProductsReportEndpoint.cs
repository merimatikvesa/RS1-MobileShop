using Ardalis.ApiEndpoints;
using backend.Data;
using backend.DTOs.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace backend.Endpoints.Reports
{
    public class ProductsReportEndpoint
        : EndpointBaseAsync
            .WithRequest<ProductsReportRequest>
            .WithActionResult
    {
        private readonly MyDbContext _db; 

        public ProductsReportEndpoint(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet("api/reports/products")]
        public override async Task<ActionResult> HandleAsync(
            [FromQuery] ProductsReportRequest request,
            CancellationToken cancellationToken = default)
        {
            // validatiion
            if (request.StartDate == default || request.EndDate == default)
                return new BadRequestObjectResult(new { errors = new[] { "StartDate and EndDate are required." } });

            if (request.StartDate > request.EndDate)
                return new BadRequestObjectResult(new { errors = new[] { "StartDate must be <= EndDate." } });
                      
            var query = _db.Products
                .AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            // ID filter 
            if (request.Id.HasValue && request.Id.Value > 0)
                query = query.Where(p => p.ProductId == request.Id.Value);

            // Name filter 
            if (!string.IsNullOrWhiteSpace(request.ProductName))
            {
                var term = request.ProductName.Trim();
                query = query.Where(p => p.ProductName.Contains(term));
            }

            // Date range 
            var startUtc = request.StartDate.Date.ToUniversalTime();
            var endExclusiveUtc = request.EndDate.Date.AddDays(1).ToUniversalTime();
            query = query.Where(p => p.CreatedAt >= startUtc && p.CreatedAt < endExclusiveUtc);

            var rows = await query
                .OrderBy(p => p.ProductId)
                .Select(p => new ProductRow(
                    p.ProductId,
                    p.ProductName,
                    p.Model,
                    p.Price,
                    p.Brand.Name,
                    p.Category.Name,
                    p.Supplier.SupplierName,
                    p.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            QuestPDF.Settings.License = LicenseType.Community;

            var parameters =
                $"Id={(request.Id?.ToString() ?? "Any")}, " +
                $"Name={(string.IsNullOrWhiteSpace(request.ProductName) ? "Any" : request.ProductName.Trim())}, " +
                $"From={request.StartDate:yyyy-MM-dd}, To={request.EndDate:yyyy-MM-dd}";

            var pdfBytes = BuildPdf("Products PDF Report", parameters, rows);

            var fileName = $"products_report_{DateTime.UtcNow:yyyyMMddHHmm}.pdf";

            return new FileContentResult(pdfBytes, "application/pdf")
            {
                FileDownloadName = fileName
            };
        }

        private static byte[] BuildPdf(string title, string parameters, List<ProductRow> rows)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(24);

                    page.Header().Column(col =>
                    {
                        col.Item().Text(title).FontSize(18).Bold();

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text(t =>
                            {
                                t.Span("Parameters: ").SemiBold();
                                t.Span(parameters);
                            });
                        });

                        col.Item().Text($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm}").FontSize(10);
                        col.Item().PaddingVertical(5).LineHorizontal(1);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Total items: {rows.Count}").FontSize(11).SemiBold();
                        col.Item().PaddingVertical(8);

                        col.Item().Table(t =>
                        {
                            static IContainer CellStyle(IContainer c) => c
                                .BorderBottom(1)
                                .BorderColor("#d9eeed")
                                .PaddingVertical(4)
                                .PaddingHorizontal(6)
                                .AlignMiddle();

                            static IContainer HeaderStyle(IContainer c) => c
                                .Background("#d9eeed")
                                .PaddingVertical(6)
                                .PaddingHorizontal(6)
                                .BorderBottom(1)
                                .BorderColor("#617c9c");

                            t.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(32);   // ID
                                c.RelativeColumn(2.2f); // Name 
                                c.RelativeColumn(1.2f); // Model
                                c.RelativeColumn(1.0f); // Price
                                c.RelativeColumn(1.2f); // Brand
                                c.RelativeColumn(1.6f); // Category 
                                c.RelativeColumn(1.8f); // Supplier 
                                c.RelativeColumn(1.4f); // Created
                            });

                            // HEADER
                            t.Header(h =>
                            {
                                h.Cell().Element(HeaderStyle).Text("ID").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Name").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Model").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Price").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Brand").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Category").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Supplier").SemiBold().FontSize(8);
                                h.Cell().Element(HeaderStyle).Text("Created").SemiBold().FontSize(8);
                            });

                            if (rows.Count == 0)
                            {
                                t.Cell().ColumnSpan(8).Element(CellStyle).Text("No data for selected parameters.").FontSize(10);
                                return;
                            }

                            // ROWS
                            for (int i = 0; i < rows.Count; i++)
                            {
                                var r = rows[i];
                                var zebra = (i % 2 == 0) ? "#f3f4f6" : "#f6f5f3";

                                IContainer RowCell(IContainer c) => CellStyle(c).Background(zebra);

                                t.Cell().Element(RowCell).Text(r.ProductId.ToString()).FontSize(10);

                                // ClampLines(1) + Ellipsis() = ne lomi, nego stavi ...
                                t.Cell().Element(RowCell).Text(r.ProductName).FontSize(10).ClampLines(1);
                                t.Cell().Element(RowCell).Text(r.Model).FontSize(10).ClampLines(1);

                                t.Cell().Element(RowCell).AlignRight().Text(r.Price.ToString("0.00")).FontSize(10);

                                t.Cell().Element(RowCell).Text(r.Brand).FontSize(10).ClampLines(1);
                                t.Cell().Element(RowCell).Text(r.Category).FontSize(10).ClampLines(1);
                                t.Cell().Element(RowCell).Text(r.Supplier).FontSize(10).ClampLines(1);

                                t.Cell().Element(RowCell).Text(r.CreatedAt.ToString("yyyy-MM-dd")).FontSize(10);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        public record ProductRow(
            int ProductId,
            string ProductName,
            string Model,
            decimal Price,
            string Brand,
            string Category,
            string Supplier,
            DateTime CreatedAt
        );
    }
}
