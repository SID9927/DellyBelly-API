namespace DellyBellyAPI.DTOs
{
    public class SiteSettingsDto
    {
        public string StoreName { get; set; }
        public string Tagline { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }
        public string Address { get; set; }
        
        // Delivery Platforms
        public string SwiggyUrl { get; set; }
        public string ZomatoUrl { get; set; }
        public string MagicPinUrl { get; set; }

        public string BusinessHoursJson { get; set; }

        public bool AnnouncementEnabled { get; set; }
        public string AnnouncementText { get; set; }
        public string AnnouncementVisibility { get; set; }
    }
}
