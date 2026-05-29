using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "super_admin,admin,manager")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductsController(IProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Server-side paginated product list.
        /// GET /api/products/paged?page=1&pageSize=12&category=Pastries&search=croissant&sortBy=low-high
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] string? category = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            var result = await _productService.GetPagedAsync(page, pageSize, category, search, sortBy, minPrice, maxPrice);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            // Project into a flat shape — avoids IgnoreCycles silently dropping nested
            // navigation properties (Product ↔ ProductIngredient ↔ Ingredient cycle)
            return Ok(new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.IsAvailable,
                product.IsBestSeller,
                product.IsRecommended,
                product.CategoryId,
                Category = product.Category == null ? null : new
                {
                    product.Category.Id,
                    product.Category.Name,
                },
                Images = product.Images?.Select(img => new
                {
                    img.Id,
                    img.FileName,
                    img.ContentType,
                }) ?? Enumerable.Empty<object>(),
                // Flat ingredient projection — no back-references, no cycles
                ProductIngredients = product.ProductIngredients?.Select(pi => new
                {
                    pi.IngredientId,
                    Ingredient = pi.Ingredient == null ? null : new
                    {
                        pi.Ingredient.Id,
                        pi.Ingredient.Name,
                        pi.Ingredient.IsAllergen,
                        pi.Ingredient.CategoryId,
                    }
                }) ?? Enumerable.Empty<object>(),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(DellyBellyAPI.DTOs.CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                IsAvailable = productDto.IsAvailable,
                IsBestSeller = productDto.IsBestSeller,
                IsRecommended = productDto.IsRecommended,
                CategoryId = productDto.CategoryId
            };

            var created = await _productService.CreateAsync(product);

            // Save ingredient links if any were selected
            if (productDto.IngredientIds?.Any() == true)
            {
                var links = productDto.IngredientIds
                    .Distinct()
                    .Select(ingredientId => new ProductIngredient
                    {
                        ProductId = created.Id,
                        IngredientId = ingredientId
                    });
                _context.ProductIngredients.AddRange(links);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DellyBellyAPI.DTOs.CreateProductDto productDto)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.Stock = productDto.Stock;
            existingProduct.IsAvailable = productDto.IsAvailable;
            existingProduct.IsBestSeller = productDto.IsBestSeller;
            existingProduct.IsRecommended = productDto.IsRecommended;
            existingProduct.CategoryId = productDto.CategoryId;
            existingProduct.Category = null; // Clear navigation property so FK update takes precedence

            var updated = await _productService.UpdateAsync(existingProduct);

            // Sync ingredient links: delete old, insert new
            var oldLinks = _context.ProductIngredients.Where(pi => pi.ProductId == id);
            _context.ProductIngredients.RemoveRange(oldLinks);

            if (productDto.IngredientIds?.Any() == true)
            {
                var newLinks = productDto.IngredientIds
                    .Distinct()
                    .Select(ingredientId => new ProductIngredient
                    {
                        ProductId = id,
                        IngredientId = ingredientId
                    });
                _context.ProductIngredients.AddRange(newLinks);
            }

            await _context.SaveChangesAsync();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id:int}/upload-photo")]
        public async Task<IActionResult> UploadProductPhoto(int id, IFormFile file)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            var image = new ImageEntity
            {
                FileName = fileName,
                ContentType = contentType,
                Data = webpBytes,
                Source = "Product",
                ProductId = id
            };

            product.Images ??= new List<ImageEntity>();
            product.Images.Add(image);

            await _productService.UpdateAsync(product);

            return Ok(new { product.Id, product.Name, image.FileName });
        }

        // Fast path: query Images table directly — no need to load the full Product entity
        [HttpGet("{id:int}/photo/{imageId:int}")]
        public async Task<IActionResult> GetProductPhoto(int id, int imageId)
        {
            var metadata = await _context.Images
                .AsNoTracking()
                .Where(i => i.Id == imageId && i.ProductId == id)
                .Select(i => new { i.UploadedAt, i.ContentType, i.FileName })
                .FirstOrDefaultAsync();

            if (metadata == null) return NotFound();

            var etag = $"\"img-{imageId}-{metadata.UploadedAt.Ticks}\"";
            Response.Headers["Cache-Control"] = "public, max-age=86400, immutable";
            Response.Headers["ETag"] = etag;

            var requestETag = Request.Headers["If-None-Match"].ToString();
            if (requestETag == etag)
            {
                return StatusCode(304);
            }

            var image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == id);

            if (image == null) return NotFound();

            return File(image.Data, image.ContentType, image.FileName);
        }

        [HttpPut("{id:int}/update-photo/{imageId:int}")]
        public async Task<IActionResult> UpdateProductPhoto(int id, int imageId, IFormFile file)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            var image = product.Images?.FirstOrDefault(i => i.Id == imageId);
            if (image == null) return NotFound();

            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            image.FileName = fileName;
            image.ContentType = contentType;
            image.Data = webpBytes;
            image.UploadedAt = DateTimeHelper.GetIndianTime();
            image.Source = "Product";

            await _productService.UpdateAsync(product);
            return Ok(new { product.Id, product.Name, image.FileName });
        }

        [HttpDelete("{id:int}/delete-photo/{imageId:int}")]
        public async Task<IActionResult> DeleteProductPhoto(int id, int imageId)
        {
            var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId && i.ProductId == id);
            if (image == null) return NotFound();

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}/set-primary/{imageId:int}")]
        public async Task<IActionResult> SetPrimaryImage(int id, int imageId)
        {
            var images = await _context.Images
                .Where(i => i.ProductId == id)
                .ToListAsync();

            var targetImage = images.FirstOrDefault(i => i.Id == imageId);
            if (targetImage == null) return NotFound("Image not found");

            var now = DateTimeHelper.GetIndianTime();

            // Set the target image's UploadedAt to an epoch date to guarantee it comes first
            targetImage.UploadedAt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Reset any other images that were previously marked as primary to the current time
            var otherImages = images.Where(i => i.Id != imageId).OrderBy(i => i.UploadedAt).ToList();
            for (int i = 0; i < otherImages.Count; i++)
            {
                if (otherImages[i].UploadedAt < new DateTime(2000, 1, 1))
                {
                    otherImages[i].UploadedAt = now.AddSeconds(i);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
