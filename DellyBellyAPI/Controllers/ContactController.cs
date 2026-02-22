using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ContactMessageDto dto)
        {
            if (dto == null) return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.FullName) || 
                string.IsNullOrWhiteSpace(dto.Email) || 
                string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { message = "Please fill in all required fields." });
            }

            var message = new ContactMessage
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Message = dto.Message,
                SubmittedAt = DateTimeHelper.GetIndianTime()
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Your message has been sent successfully! Our team will get back to you soon." });
        }

        [HttpGet("messages")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.ContactMessages
                .AsNoTracking()
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();
            return Ok(messages);
        }
    }
}
