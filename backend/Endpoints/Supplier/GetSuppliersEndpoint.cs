using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Brand
{
    public class GetSuppliersEndpoint(MyDbContext db)
       : MyEndpointBaseAsync.WithoutRequest<object>
    {
        [HttpGet("api/suppliers")]
        public override async Task<ActionResult<object>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var data = await db.Suppliers
                .OrderBy(b => b.SupplierName)
                .Select(b => new
                {
                    b.SupplierId,
                    b.SupplierName
                })
                .ToListAsync(cancellationToken);

            return Ok(data);
        }
    }
}
