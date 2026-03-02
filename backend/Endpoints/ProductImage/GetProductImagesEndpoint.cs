using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.ProductImage
{
    public class GetProductImagesEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithRequest<int, object>
    {
        [HttpGet("api/products/{id:int}/images")]
        public override async Task<ActionResult<object>> HandleAsync(int id, CancellationToken ct = default)
        {
            var data = await db.ProductImages
                .Where(pi => pi.ProductId == id)
                .Select(pi => new
                {
                    pi.ProductImageId,
                    pi.ImageId,
                    pi.Image.ImagePath
                })
                .ToListAsync(ct);

            return Ok(data);
        }
    }
}
