using System;

namespace DellyBelly.Domain.Entities
{
    public class CustomerAddress
    {
        public int Id { get; set; }                         
        public int CustomerId { get; set; }                 
        public Customer? Customer { get; set; }             

        public string Type { get; set; } = "Home";          // e.g. Home, Work, Other
        public string AddressText { get; set; } = string.Empty; // The full delivery address
        public bool IsDefault { get; set; } = false;        // Indicates if this is the primary choice
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
