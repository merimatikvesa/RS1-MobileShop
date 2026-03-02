using backend.Data;
using backend.Helper;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints.Products
{
    [Authorize(Roles = "Admin")]
    public class UploadProductImagesEndpoint(MyDbContext db, IWebHostEnvironment env)
        : MyEndpointBaseAsync.WithRequest<int, object>
    {
        [HttpPost("api/products/{id:int}/images")]
        public override async Task<ActionResult<object>> HandleAsync(int id, CancellationToken ct = default)
        {
            
            var productExists = await db.Products.AnyAsync(p => p.ProductId == id, ct);
            if (!productExists) return NotFound(new { error = "Product not found." });

            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
                return BadRequest(new { error = "No files uploaded." });

        
            var allowed = new HashSet<string> { "image/jpeg", "image/png", "image/webp" };
            const long maxSize = 3 * 1024 * 1024; 

            
            var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadDir = Path.Combine(webRoot, "uploads", "products", id.ToString());
            Directory.CreateDirectory(uploadDir);

            var result = new List<object>();

            foreach (var file in files)
            {
                if (!allowed.Contains(file.ContentType))
                    return BadRequest(new { error = "Only JPG/PNG/WEBP allowed." });

                if (file.Length <= 0 || file.Length > maxSize)
                    return BadRequest(new { error = "Each image must be <= 3MB." });

                var ext = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(uploadDir, fileName);

                await using (var stream = System.IO.File.Create(fullPath))
                    await file.CopyToAsync(stream, ct);

              
                var relativePath = $"/uploads/products/{id}/{fileName}";

               
                var image = new Image { ImagePath = relativePath };
                db.Images.Add(image);
                await db.SaveChangesAsync(ct); 

                
                var pi = new backend.Models.ProductImage
                {
                    ProductId = id,
                    ImageId = image.ImageId
                };
                db.ProductImages.Add(pi);

                result.Add(new { image.ImageId, image.ImagePath });
            }

            await db.SaveChangesAsync(ct);

            return Ok(new
            {
                message = "Images uploaded.",
                images = result
            });
        }
    }
}