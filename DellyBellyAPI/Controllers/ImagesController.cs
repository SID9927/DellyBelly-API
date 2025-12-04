using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DellyBelly.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var webpBytes = await ImageHelper.ConvertToWebPAsync(stream);

            var imageEntity = new ImageEntity
            {
                FileName = Path.GetFileNameWithoutExtension(file.FileName) + ".webp",
                ContentType = "image/webp",
                Data = webpBytes
            };

            _context.Images.Add(imageEntity);
            await _context.SaveChangesAsync();

            return Ok(new { imageEntity.Id, imageEntity.FileName });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            return File(image.Data, image.ContentType, image.FileName);
        }
    }

}
