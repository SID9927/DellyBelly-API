using System;
using System.Collections.Generic;
using System.Text;
using DellyBelly.Shared.Helpers;

namespace DellyBelly.Domain.Entities
{
    public class SiteSettings
    {
        public int Id { get; set; }                  // Primary Key (usually 1 for singleton)
        public string StoreName { get; set; }        // name
        public string Tagline { get; set; }          // tagline
        public string Email { get; set; }           // email
        public string Phone { get; set; }           // phone
        public string WhatsApp { get; set; }        // whatsapp
        public string Address { get; set; }         // address
        
        // Delivery Platforms
        public string SwiggyUrl { get; set; }
        public string ZomatoUrl { get; set; }
        public string MagicPinUrl { get; set; }     // optional extra

        // Business Hours (could be a stringified JSON if keeping it simple, 
        // OR separate fields if required. Sticking with simple for now)
        public string BusinessHoursJson { get; set; }

        // Announcement Ribbon
        public bool AnnouncementEnabled { get; set; } = false;
        public string AnnouncementText { get; set; } = "";
        public string AnnouncementVisibility { get; set; } = "both"; // "before", "after", "both"

        public DateTime UpdatedAt { get; set; } = DateTimeHelper.GetIndianTime();
        public string? UpdatedBy { get; set; }
    }
}
