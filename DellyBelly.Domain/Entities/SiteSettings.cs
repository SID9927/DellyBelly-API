using System;
using System.Collections.Generic;
using System.Text;
using DellyBelly.Shared.Helpers;

namespace DellyBelly.Domain.Entities
{
    public class SiteSettings
    {
        public int Id { get; set; }
        public string? StoreName { get; set; }
        public string? Tagline { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? WhatsApp { get; set; }
        public string? Address { get; set; }
        
        public string? SwiggyUrl { get; set; }
        public string? ZomatoUrl { get; set; }
        public string? MagicPinUrl { get; set; }

        public string? BusinessHoursJson { get; set; }

        public bool AnnouncementEnabled { get; set; } = false;
        public string? AnnouncementText { get; set; } = "";
        public string? AnnouncementVisibility { get; set; } = "both";

        public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetIndianTime();
        public string? UpdatedBy { get; set; }
    }
}
