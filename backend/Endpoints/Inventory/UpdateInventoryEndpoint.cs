using backend.Data;
using backend.Helper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Inventories
{
    [AllowAnonymous]
    public class UpdateInventoryEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<UpdateInventoryRequest, object>
    {
        [HttpPut("api/inventory/{productId}")]
        public override async Task<ActionResult<object>> HandleAsync(
            UpdateInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var inventory = await db.Inventory
                .FirstOrDefaultAsync(i => i.ProductId == request.ProductId, cancellationToken);

            if (inventory == null)
            {
                return NotFound(new { error = "Inventory not found." });
            }

            if (request.QuantityInStock < 0)
            {
                return BadRequest(new { error = "Quantity cannot be negative." });
            }

            inventory.QuantityInStock = request.QuantityInStock;
            inventory.LastUpdated = DateTime.Now;

            await db.SaveChangesAsync(cancellationToken);

            return Ok(inventory);
        }
    }

    public class UpdateInventoryRequest
    {
        public int ProductId { get; set; }
        public int QuantityInStock { get; set; }
    }
}