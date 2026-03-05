using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Helper;
using backend.DTOs.Inventory;


namespace backend.Endpoints.Inventories
{
    [AllowAnonymous]
    public class GetInventoryEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<GetInventoryFilterRequest, object>
    {
        [HttpGet("api/inventory")]
        public override async Task<ActionResult<object>> HandleAsync(
        [FromQuery] GetInventoryFilterRequest request,
         CancellationToken cancellationToken = default)
        {
            // BE validation
            if (request.MinQuantity < 0 || request.MaxQuantity < 0)
                return BadRequest("Quantity cannot be negative.");

            if (request.MinQuantity > request.MaxQuantity)
                return BadRequest("MinQuantity cannot be greater than MaxQuantity.");

            var query = db.Inventory
                .Include(i => i.Product)
                .AsQueryable();

            // Filters (5)
            if (request.ProductId.HasValue)
                query = query.Where(i => i.ProductId == request.ProductId);

            if (request.MinQuantity.HasValue)
                query = query.Where(i => i.QuantityInStock >= request.MinQuantity);

            if (request.MaxQuantity.HasValue)
                query = query.Where(i => i.QuantityInStock <= request.MaxQuantity);

            if (!string.IsNullOrWhiteSpace(request.ProductName))
                query = query.Where(i =>
                    i.Product.ProductName.Contains(request.ProductName));

            if (request.IsLowStock == true)
                query = query.Where(i => i.QuantityInStock < 5);

            var totalCount = await query.CountAsync(cancellationToken);
            var inventory = await query
                .Skip((request.PageNumber - 1)* request.PageSize)
                .Take(request.PageSize)
                .Select(i => new
                {
                    i.ProductId,
                    ProductName = i.Product.ProductName,
                    Quantity = i.QuantityInStock,
                    i.LastUpdated
                })
                .ToListAsync(cancellationToken);

            return Ok(new
            {
                data = inventory,
                totalCount,
                pageNumber = request.PageNumber,
                pageSize=request.PageSize

            }
                );
        }
    }
}