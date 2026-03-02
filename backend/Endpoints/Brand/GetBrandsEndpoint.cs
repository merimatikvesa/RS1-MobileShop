using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Brand
{
    public class GetBrandsEndpoint(MyDbContext db)
       : MyEndpointBaseAsync.WithoutRequest<object>
    {
        [HttpGet("api/brands")]
        public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var data = await db.Brands
                .OrderBy(b => b.Name)
                .Select(b => new
                {
                    b.BrandId,
                    b.Name
                })
                .ToListAsync(cancellationToken);

            return Ok(data);
        }
    }
}
