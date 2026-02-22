using System;

namespace DellyBelly.Domain.Entities
{
    public class NewsletterSubscription
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime SubscribedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        public bool IsActive { get; set; } = true;
    }
}
