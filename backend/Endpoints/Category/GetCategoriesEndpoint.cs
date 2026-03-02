using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Categories
{
    public class GetCategoriesEndpoint(MyDbContext db)
        : MyEndpointBaseAsync.WithoutRequest<object>
    {
        [HttpGet("api/categories")]
        public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var data = await db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.CategoryId,
                    c.Name
                })
                .ToListAsync(cancellationToken);

            return Ok(data);
        }
    }
}