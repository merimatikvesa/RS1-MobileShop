using backend.Data;
using backend.Helper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Inventories
{
    [AllowAnonymous]
    public class CreateInventoryEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<CreateInventoryRequest, object>
    {
        [HttpPost("api/inventory")]
        public override async Task<ActionResult<object>> HandleAsync(
            CreateInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            // Check if Product exists
            var productExists = await db.Products
                .AnyAsync(p => p.ProductId == request.ProductId, cancellationToken);

            if (!productExists)
            {
                return BadRequest(new { error = "Product does not exist." });
            }

            // Check if Inventory already exists
            var inventoryExists = await db.Inventory
                .AnyAsync(i => i.ProductId == request.ProductId, cancellationToken);

            if (inventoryExists)
            {
                return BadRequest(new { error = "Inventory already exists for this product." });
            }

            if (request.QuantityInStock < 0)
            {
                return BadRequest(new { error = "Quantity cannot be negative." });
            }

            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                QuantityInStock = request.QuantityInStock,
                LastUpdated = DateTime.Now
            };

            db.Inventory.Add(inventory);
            await db.SaveChangesAsync(cancellationToken);

            return Ok(inventory);
        }
    }

    public class CreateInventoryRequest
    {
        public int ProductId { get; set; }
        public int QuantityInStock { get; set; }
    }
}