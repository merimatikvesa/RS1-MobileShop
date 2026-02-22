using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Inventories
{
    [AllowAnonymous]
    public class DeleteInventoryEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<DeleteInventoryRequest, object>
    {
        [HttpDelete("api/inventory/{productId}")]
        public override async Task<ActionResult<object>> HandleAsync(
            DeleteInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var inventory = await db.Inventory
                .FirstOrDefaultAsync(i => i.ProductId == request.ProductId, cancellationToken);

            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventory.Remove(inventory);
            await db.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }

    public class DeleteInventoryRequest
    {
        public int ProductId { get; set; }
    }
}