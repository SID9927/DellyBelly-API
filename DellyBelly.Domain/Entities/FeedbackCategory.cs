using System;
using System.Collections.Generic;

namespace DellyBelly.Domain.Entities
{
    public class FeedbackCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        // Navigation property
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
