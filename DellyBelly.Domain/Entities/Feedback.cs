using System;

namespace DellyBelly.Domain.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key to Customer
        public int CategoryId { get; set; } // Foreign Key to FeedbackCategory
        public string Subject { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1-5
        public bool IsApproved { get; set; } = false;
        public DateTime SubmittedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        // Navigation properties
        public Customer? User { get; set; }
        public FeedbackCategory? Category { get; set; }
    }
}
