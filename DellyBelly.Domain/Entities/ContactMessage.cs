using System;
using DellyBelly.Shared.Helpers;

namespace DellyBelly.Domain.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; } = DateTimeHelper.GetIndianTime();
        public bool IsRead { get; set; } = false;
    }
}
