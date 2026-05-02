using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Application.Interfaces;
using DellyBellyAPI.DTOs;
using DellyBelly.Shared.Helpers;
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
        private readonly IEmailService _emailService;

        public NewsletterController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            
            // Send a "Welcome" email automatically!
            string welcomeBody = EmailTemplateHelper.GetNewsletterTemplate(
                "<h3>Thank you for joining our Delly Belly family!</h3><p>We're thrilled to have you here. We'll notify you about our fresh daily bakes, special cake offers, and seasonal treats.</p><p>Welcome to the sweetest community in town!</p>", 
                "Welcome to Delly Belly! 🥐");

            await _emailService.SendEmailAsync(emailLower, "Welcome to Delly Belly! 🥐", welcomeBody, "Newsletter Subscription");

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

        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast([FromBody] BroadcastDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Subject) || string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "Subject and Content are required for broadcast." });

            List<string> targets;

            if (dto.TargetIds != null && dto.TargetIds.Any())
            {
                // Send to specific IDs
                targets = await _context.NewsletterSubscriptions
                    .Where(s => dto.TargetIds.Contains(s.Id) && s.IsActive)
                    .Select(s => s.Email)
                    .ToListAsync();
            }
            else
            {
                // Send to ALL active subscribers
                targets = await _context.NewsletterSubscriptions
                    .Where(s => s.IsActive)
                    .Select(s => s.Email)
                    .ToListAsync();
            }

            if (!targets.Any())
                return BadRequest(new { message = "No active subscribers found for this broadcast." });

            // Wrap the content in our premium template
            string styledContent = EmailTemplateHelper.GetNewsletterTemplate(dto.Content, dto.Subject);

            int count = await _emailService.SendBroadcastAsync(targets, dto.Subject, styledContent, "Newsletter Broadcast");

            return Ok(new { message = $"Broadcast sent successfully to {count} subscribers.", sentCount = count });
        }

        [HttpGet("quota")]
        public async Task<IActionResult> GetQuotaStatus()
        {
            var remaining = await _emailService.GetRemainingQuotaAsync();
            return Ok(new { remaining });
        }
    }

    public class BroadcastDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<int>? TargetIds { get; set; }
    }
}
