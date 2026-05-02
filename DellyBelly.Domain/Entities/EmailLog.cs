using System;
using DellyBelly.Shared.Helpers;

namespace DellyBelly.Domain.Entities
{
    public class EmailLog
    {
        public int Id { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty; // e.g. "Newsletter Broadcast", "Contact Form Receipt", "Admin Notification"
        public DateTime SentAt { get; set; } = DateTimeHelper.GetIndianTime();
        public bool IsSuccess { get; set; }
    }
}
