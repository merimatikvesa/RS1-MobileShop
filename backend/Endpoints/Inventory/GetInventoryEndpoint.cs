using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Helper;

namespace backend.Endpoints.Inventories
{
    [AllowAnonymous]
    public class GetInventoryEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithoutRequest<object>
    {
        [HttpGet("api/inventory")]
        public override async Task<ActionResult<object>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var inventory = await db.Inventory
                .Include(i => i.Product)
                .Select(i => new
                {
                    i.ProductId,
                    ProductName = i.Product.ProductName,
                    Quantity = i.QuantityInStock,
                    i.LastUpdated
                })
                .ToListAsync(cancellationToken);

            return Ok(inventory);
        }
    }
}