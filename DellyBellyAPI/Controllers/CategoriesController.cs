using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Super_Admin,Admin,Manager")]
    public class CategoriesController :ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
                Description = categoryDto.Description,
                IsActive = categoryDto.IsActive
            };

            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            var updatedCategory = await _categoryService.UpdateAsync(category);
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

        [HttpGet("{id}/photo")]
        public async Task<IActionResult> GetCategoryPhoto(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category?.Image == null) return NotFound();

            return File(category.Image.Data, category.Image.ContentType, category.Image.FileName);
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
                category.Image.UploadedAt = DateTime.UtcNow;
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
