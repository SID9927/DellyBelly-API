using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "super_admin,admin,manager")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
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
                CategoryId = productDto.CategoryId
            };

            var created = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            var updated = await _productService.UpdateAsync(product);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/upload-photo")]
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

        [HttpGet("{id}/photo/{imageId}")]
        public async Task<IActionResult> GetProductPhoto(int id, int imageId)
        {
            var product = await _productService.GetByIdAsync(id);
            var image = product?.Images?.FirstOrDefault(i => i.Id == imageId);
            if (image == null) return NotFound();

            return File(image.Data, image.ContentType, image.FileName);
        }

        [HttpPut("{id}/update-photo/{imageId}")]
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
            image.UploadedAt = DateTime.UtcNow;
            image.Source = "Product";

            await _productService.UpdateAsync(product);
            return Ok(new { product.Id, product.Name, image.FileName });
        }

        [HttpDelete("{id}/delete-photo/{imageId}")]
        public async Task<IActionResult> DeleteProductPhoto(int id, int imageId)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            var image = product.Images?.FirstOrDefault(i => i.Id == imageId);
            if (image == null) return NotFound();

            product.Images.Remove(image);
            await _productService.UpdateAsync(product);

            return NoContent();
        }
    }
}
