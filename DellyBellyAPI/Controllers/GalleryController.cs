using DellyBelly.Application.Interfaces;
using DellyBelly.Application.DTOs;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;

        public GalleryController(IGalleryService galleryService, ApplicationDbContext context)
        {
            _galleryService = galleryService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var galleries = await _galleryService.GetAllAsync();
            // Projecting out the heavy 'Data' byte array to prevent multi-megabyte JSON bloat on load
            var result = galleries.Select(g => new
            {
                id = g.Id,
                fileName = g.FileName,
                contentType = g.ContentType,
                isActive = g.IsActive
            });
            return Ok(result);
        }

        [HttpGet("debug-count")]
        public async Task<IActionResult> GetCount()
        {
            var galleries = await _galleryService.GetAllAsync();
            return Ok(new { count = galleries.Count() });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gallery = await _galleryService.GetByIdAsync(id);
            if (gallery == null) return NotFound();
            
            return Ok(new
            {
                id = gallery.Id,
                fileName = gallery.FileName,
                contentType = gallery.ContentType,
                isActive = gallery.IsActive
            });
        }

        [HttpGet("{id}/photo")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var metadata = await _context.Galleries
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new { g.UploadedAt, g.ContentType, g.FileName })
                .FirstOrDefaultAsync();

            if (metadata == null) return NotFound();

            var etag = $"\"gal-{id}-{metadata.UploadedAt.Ticks}\"";
            Response.Headers["Cache-Control"] = "public, max-age=86400, immutable";
            Response.Headers["ETag"] = etag;

            var requestETag = Request.Headers["If-None-Match"].ToString();
            if (requestETag == etag)
            {
                return StatusCode(304);
            }

            var gallery = await _galleryService.GetByIdAsync(id);
            if (gallery == null || gallery.Data == null) return NotFound();

            return File(gallery.Data, gallery.ContentType, gallery.FileName);
        }

        // Renamed to Upload for clarity as each upload IS a gallery item now
        [HttpPost] 
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var (webpBytes, fileName, contentType) = await ImageHelper.CreateWebPImageAsync(stream, file.FileName);

            var gallery = new Gallery
            {
                IsActive = true,
                FileName = fileName,
                ContentType = contentType,
                Data = webpBytes
            };

            var created = await _galleryService.CreateAsync(gallery);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGalleryDto dto)
        {
            var existing = await _galleryService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Only update mutable fields (IsActive)
            existing.IsActive = dto.IsActive;

            var updated = await _galleryService.UpdateAsync(existing);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _galleryService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
 