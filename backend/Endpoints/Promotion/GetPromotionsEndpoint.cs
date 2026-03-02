using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Brand
{
    public class GetPromotionsEndpoint(MyDbContext db)
       : MyEndpointBaseAsync.WithoutRequest<object>
    {
        [HttpGet("api/promotions")]
        public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var data = await db.Promotions
                .OrderBy(b => b.PromotionName)
                .Select(b => new
                {
                    b.PromotionId,
                    b.PromotionName
                })
                .ToListAsync(cancellationToken);

            return Ok(data);
        }
    }
}
