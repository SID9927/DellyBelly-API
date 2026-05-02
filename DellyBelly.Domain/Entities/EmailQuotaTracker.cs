using System;

namespace DellyBelly.Domain.Entities
{
    public class EmailQuotaTracker
    {
        public int Id { get; set; }
        public DateTime LastResetDate { get; set; }
        public int EmailsSentCount { get; set; }
    }
}
