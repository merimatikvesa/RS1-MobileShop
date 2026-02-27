using backend.Data;
using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Products
{
    public class GetProductsEndpoint(MyDbContext db) : MyEndpointBaseAsync.WithRequest<GetProductsFilterRequest, object>
    {
        [HttpGet("api/products")]
        public override async Task<ActionResult<object>> HandleAsync(
            [FromQuery] GetProductsFilterRequest request,
            CancellationToken cancellationToken = default)
        {
            // BE validation
            if (request.PageNumber < 1) return BadRequest("PageNumber must be >= 1.");
            if (request.PageSize < 1 || request.PageSize > 100) return BadRequest("PageSize must be between 1 and 100.");

            if (request.MinPrice < 0 || request.MaxPrice < 0)
                return BadRequest("Price cannot be negative.");

            if (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice)
                return BadRequest("MinPrice cannot be greater than MaxPrice.");

            var query = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsQueryable();

            // Filters 
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var s = request.Search.Trim();
                if (s.Length < 2) return BadRequest("Search must be at least 2 characters.");
                query = query.Where(p => p.ProductName.Contains(s) || p.Model.Contains(s));
            }

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId);

            if (request.BrandId.HasValue)
                query = query.Where(p => p.BrandId == request.BrandId);

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice);

            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderBy(p => p.ProductId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.Model,
                    p.Price,
                    p.BrandId,
                    BrandName = p.Brand.Name,         
                    p.CategoryId,
                    CategoryName = p.Category.Name      
                })
                .ToListAsync(cancellationToken);

            return Ok(new
            {
                data,
                totalCount,
                pageNumber = request.PageNumber,
                pageSize = request.PageSize
            });
        }
    }
}
