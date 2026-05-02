using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Application.Interfaces;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public ContactController(ApplicationDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
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

            // 1. Notify the Bakery (Admin)
            var adminEmail = _config["EmailSettings:FromEmail"];
            string adminBody = EmailTemplateHelper.GetContactAdminTemplate(dto.FullName, dto.Email, dto.PhoneNumber, dto.Message);
            
            await _emailService.SendEmailAsync(adminEmail, $"[New Inquiry] {dto.FullName} sent a message", adminBody, "Admin Notification");

            // 2. Send Automated Receipt to the User
            string userBody = EmailTemplateHelper.GetContactUserReceiptTemplate(dto.FullName);
            
            await _emailService.SendEmailAsync(dto.Email, "We received your message - Delly Belly Bakery", userBody, "Contact Receipt");

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

        [HttpPost("reply")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> ReplyToMessage([FromBody] ContactReplyDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { message = "Recipient email and message content are required." });
            }

            // Wrap the reply in a professional template
            string subject = string.IsNullOrWhiteSpace(dto.Subject) ? "Reply from Delly Belly Bakery" : dto.Subject;
            string body = EmailTemplateHelper.GetAdminReplyTemplate(dto.RecipientName ?? "Customer", dto.Message);

            bool success = await _emailService.SendEmailAsync(dto.Email, subject, body, "Admin Reply");

            if (success)
            {
                return Ok(new { message = $"Reply sent successfully to {dto.Email}" });
            }

            return StatusCode(500, new { message = "Failed to send email. Please check your SMTP settings." });
        }
    }
}
