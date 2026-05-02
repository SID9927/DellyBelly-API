using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Create default settings if not exists
                settings = new SiteSettings
                {
                    StoreName = "Delly Belly Bakery",
                    Tagline = "Freshly Baked with Love",
                    Email = "contact@dellybelly.com",
                    Phone = "919999999999",
                    WhatsApp = "919999999999",
                    Address = "123 Baker Street, Mumbai",
                    SwiggyUrl = "",
                    ZomatoUrl = "",
                    MagicPinUrl = "",
                    BusinessHoursJson = "{}",
                    AnnouncementEnabled = false,
                    AnnouncementText = "",
                    AnnouncementVisibility = "both"
                };
                _context.SiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return Ok(settings);
        }

        [HttpPut]
        [Authorize(Roles = "super_admin,admin,manager,staff")]
        public async Task<IActionResult> Update(SiteSettingsDto dto)
        {
            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SiteSettings();
                _context.SiteSettings.Add(settings);
            }

            var adminName = User.FindFirstValue("name") ?? User.Identity?.Name ?? "Admin";

            settings.StoreName = dto.StoreName;
            settings.Tagline = dto.Tagline;
            settings.Email = dto.Email;
            settings.Phone = dto.Phone;
            settings.WhatsApp = dto.WhatsApp;
            settings.Address = dto.Address;
            settings.SwiggyUrl = dto.SwiggyUrl;
            settings.ZomatoUrl = dto.ZomatoUrl;
            settings.MagicPinUrl = dto.MagicPinUrl;
            settings.BusinessHoursJson = dto.BusinessHoursJson;
            settings.AnnouncementEnabled = dto.AnnouncementEnabled;
            settings.AnnouncementText = dto.AnnouncementText ?? "";
            settings.AnnouncementVisibility = dto.AnnouncementVisibility ?? "both";
            settings.UpdatedAt = DateTimeHelper.GetIndianTime();
            settings.UpdatedBy = adminName;

            await _context.SaveChangesAsync();
            return Ok(settings);
        }
    }
}
