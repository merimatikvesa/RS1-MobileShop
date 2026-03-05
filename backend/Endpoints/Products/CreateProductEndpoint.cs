using backend.Data;
using backend.Helper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Products
{
    [Authorize(Roles = "Admin")]
    public class CreateProductEndpoint(MyDbContext db) 
        : MyEndpointBaseAsync.WithRequest<CreateProductRequest, object>
    {
        [HttpPost("api/products")]
        public override async Task<ActionResult<object>> HandleAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            // BE validation
            if (string.IsNullOrWhiteSpace(request.ProductName))
                return BadRequest(new { error = "ProductName is required." });

            if (request.ProductName.Trim().Length < 2 || request.ProductName.Trim().Length > 100)
                return BadRequest(new { error = "ProductName must be between 2 and 100 characters." });

            if (string.IsNullOrWhiteSpace(request.Model))
                return BadRequest(new { error = "Model is required." });

            if (request.Model.Trim().Length < 1 || request.Model.Trim().Length > 100)
                return BadRequest(new { error = "Model must be between 1 and 100 characters." });

            if (request.Price < 0)
                return BadRequest(new { error = "Price cannot be negative." });

            // FK existence checks
            var brandExists = await db.Brands.AnyAsync(b => b.BrandId == request.BrandId, cancellationToken);
            if (!brandExists) return BadRequest(new { error = "Brand does not exist." });

            var categoryExists = await db.Categories.AnyAsync(c => c.CategoryId == request.CategoryId, cancellationToken);
            if (!categoryExists) return BadRequest(new { error = "Category does not exist." });

            var supplierExists = await db.Suppliers.AnyAsync(s => s.SupplierId == request.SupplierId, cancellationToken);
            if (!supplierExists) return BadRequest(new { error = "Supplier does not exist." });

            if (request.PromotionId.HasValue)
            {
                var promoExists = await db.Promotions.AnyAsync(p => p.PromotionId == request.PromotionId.Value, cancellationToken);
                if (!promoExists) return BadRequest(new { error = "Promotion does not exist." });
            }

            //prevent duplicates (name+model)
            var duplicate = await db.Products.AnyAsync(p =>
                p.ProductName == request.ProductName.Trim() &&
                p.Model == request.Model.Trim(),
                cancellationToken);

            if (duplicate)
                return BadRequest(new { error = "Product with the same name and model already exists." });

            var product = new Product
            {
                ProductName = request.ProductName.Trim(),
                Model = request.Model.Trim(),
                Price = request.Price,
                BrandId = request.BrandId,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId,
                PromotionId = request.PromotionId,
                Images = string.IsNullOrEmpty(request.ImagePath)
                         ? new List<backend.Models.ProductImage>()
    :                    new List<backend.Models.ProductImage>
                         {
                             new backend.Models.ProductImage
                             {
                                   Image = new backend.Models.Image
                                   {
                                        ImagePath = request.ImagePath
                                   }
                             }
                          }
                // Phone stay null (not required)
            };

            db.Products.Add(product);
            await db.SaveChangesAsync(cancellationToken);

            db.Inventory.Add(new Inventory
            {
                ProductId = product.ProductId,
                QuantityInStock = 0,
                LastUpdated = DateTime.UtcNow
            });

            await db.SaveChangesAsync(cancellationToken);

            // Return a clean DTO-like response (not whole EF graph)
            return Ok(new
            {
                product.ProductId,
                product.ProductName,
                product.Model,
                product.Price,
                product.BrandId,
                product.CategoryId,
                product.SupplierId,
                product.PromotionId
            });
        }
    }

    public class CreateProductRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public int? PromotionId { get; set; }
        public string ImagePath { get; set; } = "";
    }
}
