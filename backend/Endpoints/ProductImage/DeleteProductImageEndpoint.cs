using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.ProductImage
{
    [Authorize(Roles = "Admin")]
    public class DeleteProductImageEndpoint(MyDbContext db)
    : MyEndpointBaseAsync.WithRequest<(int productId, int productImageId), object>
    {
        [HttpDelete("api/products/{productId:int}/images/{productImageId:int}")]
        public override async Task<ActionResult<object>> HandleAsync(
            (int productId, int productImageId) request,
            CancellationToken ct = default)
        {
            var (productId, productImageId) = request;

            var pi = await db.ProductImages
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.ProductImageId == productImageId && x.ProductId == productId, ct);

            if (pi == null)
                return NotFound(new { error = "Image link not found." });

            var imageId = pi.ImageId;

            // remove link ProductImage
            db.ProductImages.Remove(pi);

            // OPTIONAL: delete Image row if not used by any other ProductImage
            var usedElsewhere = await db.ProductImages.AnyAsync(x => x.ImageId == imageId && x.ProductImageId != productImageId, ct);
            if (!usedElsewhere)
            {
                var img = await db.Images.FirstOrDefaultAsync(x => x.ImageId == imageId, ct);
                if (img != null) db.Images.Remove(img);
            }

            await db.SaveChangesAsync(ct);
            return Ok(new { message = "Image deleted." });
        }
    }
}
