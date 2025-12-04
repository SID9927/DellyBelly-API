using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var galleries = await _galleryService.GetAllAsync();
            return Ok(galleries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gallery = await _galleryService.GetByIdAsync(id);
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DellyBellyAPI.DTOs.CreateGalleryDto galleryDto)
        {
            var gallery = new Gallery
            {
                Name = galleryDto.Name,
                Description = galleryDto.Description,
                IsActive = galleryDto.IsActive
            };

            var created = await _galleryService.CreateAsync(gallery);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Gallery gallery)
        {
            if (id != gallery.Id) return BadRequest();
            var updated = await _galleryService.UpdateAsync(gallery);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _galleryService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/upload-photo")]
        public async Task<IActionResult> UploadGalleryPhoto(int id, IFormFile file)
        {
            var gallery = await _galleryService.GetByIdAsync(id);
            if (gallery == null) return NotFound();

            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            var image = new ImageEntity
            {
                FileName = fileName,
                ContentType = contentType,
                Data = webpBytes,
                Source = "Gallery",
                GalleryId = id
            };

            gallery.Images ??= new List<ImageEntity>();
            gallery.Images.Add(image);

            await _galleryService.UpdateAsync(gallery);

            return Ok(new { gallery.Id, gallery.Name, image.FileName });
        }
    }
}
