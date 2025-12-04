using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }                         // Primary Key
        public string FullName { get; set; }                // Full name
        public string Email { get; set; }                   // Unique email
        public string PhoneNumber { get; set; }             // Optional phone
        public string Address { get; set; }                 // Delivery address
        public decimal TotalSpent { get; set; } = 0;        // Lifetime spend
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation (for future Order linking)
        //public ICollection<Order>? Orders { get; set; }

    }
}
