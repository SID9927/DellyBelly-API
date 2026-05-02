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
        public string PasswordHash { get; set; } = string.Empty; // For user authentication
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Navigation
        //public ICollection<Order>? Orders { get; set; }
        public ICollection<CustomerAddress>? CustomerAddresses { get; set; }

    }
}
