using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] NewsletterSubscribeDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required." });

            var emailLower = dto.Email.Trim().ToLower();

            var existing = await _context.NewsletterSubscriptions
                .FirstOrDefaultAsync(s => s.Email.ToLower() == emailLower);

            if (existing != null)
            {
                if (existing.IsActive)
                    return Ok(new { message = "You are already subscribed to our newsletter!" });
                
                existing.IsActive = true;
                existing.SubscribedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                _context.NewsletterSubscriptions.Update(existing);
            }
            else
            {
                var subscription = new NewsletterSubscription
                {
                    Email = emailLower,
                    IsActive = true,
                    SubscribedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
                };
                _context.NewsletterSubscriptions.Add(subscription);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Thank you for subscribing to Delly Belly updates!" });
        }

        [HttpGet("subscribers")]
        // Consider adding [Authorize] here for admin only access later
        public async Task<IActionResult> GetSubscribers()
        {
            var subscribers = await _context.NewsletterSubscriptions
                .AsNoTracking()
                .OrderByDescending(s => s.SubscribedAt)
                .ToListAsync();
            return Ok(subscribers);
        }
    }
}
