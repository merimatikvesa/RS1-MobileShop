using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Products
{
    [Authorize(Roles = "Admin")]
    public class DeleteProductEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<int, object>
    {
        [HttpDelete("api/products/{id:int}")]
        public override async Task<ActionResult<object>> HandleAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                return BadRequest(new { error = "Invalid id." });

            var product = await db.Products
                .FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);

            if (product == null)
                return NotFound(new { error = "Product not found." });

            // Prevent delete if referenced (da ne pukne FK)
            var hasOrderItems = await db.OrderItems.AnyAsync(oi => oi.ProductId == id, cancellationToken);
            if (hasOrderItems)
                return BadRequest(new { error = "Cannot delete product because it is used in orders." });

            var hasFavorites = await db.Favorites.AnyAsync(f => f.ProductId == id, cancellationToken);
            if (hasFavorites)
                return BadRequest(new { error = "Cannot delete product because it is in favorites." });

            // Optional: remove related inventory/phone/images first if you want hard delete
            // (Ako imate cascade u bazi, ovo ti nije potrebno.)
            var inventory = await db.Inventory.FirstOrDefaultAsync(i => i.ProductId == id, cancellationToken);
            if (inventory != null) db.Inventory.Remove(inventory);

            var phone = await db.Phones.FirstOrDefaultAsync(ph => ph.ProductId == id, cancellationToken);
            if (phone != null) db.Phones.Remove(phone);

            db.Products.Remove(product);

            try
            {
                await db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { error = "Cannot delete product due to related data (FK constraint)." });
            }

            return Ok(new { message = "Product deleted successfully." });
        }
    }
}
