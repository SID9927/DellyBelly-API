using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Super_Admin,Admin,Manager")]
    public class CategoriesController :ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ApplicationDbContext _context;

        public CategoriesController(ICategoryService categoryService, ApplicationDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DellyBellyAPI.DTOs.CreateCategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description ?? string.Empty,
                IsActive = categoryDto.IsActive
            };

            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DellyBellyAPI.DTOs.CreateCategoryDto categoryDto)
        {
            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description ?? string.Empty;
            existingCategory.IsActive = categoryDto.IsActive;

            var updatedCategory = await _categoryService.UpdateAsync(existingCategory);
            return Ok(updatedCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/upload-photo")]
        public async Task<IActionResult> UploadCategoryPhoto(int id, IFormFile file)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            var image = new ImageEntity
            {
                FileName = fileName,
                ContentType = contentType,
                Data = webpBytes,
                Source = "Category",
                CategoryId = id
            };

            category.Image = image;
            await _categoryService.UpdateAsync(category);

            return Ok(new { category.Id, category.Name, image.FileName });
        }

        // Fast path: query Images table directly — avoids loading the entire Category entity
        [HttpGet("{id}/photo")]
        public async Task<IActionResult> GetCategoryPhoto(int id)
        {
            var metadata = await _context.Images
                .AsNoTracking()
                .Where(i => i.CategoryId == id && i.Source == "Category")
                .Select(i => new { i.Id, i.UploadedAt, i.ContentType, i.FileName })
                .FirstOrDefaultAsync();

            if (metadata == null) return NotFound();

            var etag = $"\"cat-{id}-{metadata.UploadedAt.Ticks}\"";
            Response.Headers["Cache-Control"] = "public, max-age=86400, immutable";
            Response.Headers["ETag"] = etag;

            var requestETag = Request.Headers["If-None-Match"].ToString();
            if (requestETag == etag)
            {
                return StatusCode(304);
            }

            var image = await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == metadata.Id);

            if (image?.Data == null) return NotFound();

            return File(image.Data, image.ContentType, image.FileName);
        }

        [HttpPut("{id}/update-photo")]
        public async Task<IActionResult> UpdateCategoryPhoto(int id, IFormFile file)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            // If category already has an image, replace it
            if (category.Image != null)
            {
                category.Image.FileName = fileName;
                category.Image.ContentType = contentType;
                category.Image.Data = webpBytes;
                category.Image.UploadedAt = DateTimeHelper.GetIndianTime();
                category.Image.Source = "Category";
            }
            else
            {
                category.Image = new ImageEntity
                {
                    FileName = fileName,
                    ContentType = contentType,
                    Data = webpBytes,
                    Source = "Category",
                    CategoryId = id
                };
            }

            await _categoryService.UpdateAsync(category);
            return Ok(new { category.Id, category.Name, category.Image.FileName });
        }

        [HttpDelete("{id}/delete-photo")]
        public async Task<IActionResult> DeleteCategoryPhoto(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null || category.Image == null) return NotFound();

            category.Image = null; // remove reference
            await _categoryService.UpdateAsync(category);

            return NoContent();
        }
    }
}
