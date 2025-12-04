using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }                     // Primary Key
        public string Name { get; set; }                // Product name
        public string Description { get; set; }         // Description
        public decimal Price { get; set; }              // Price in INR
        public int Stock { get; set; }                  // Inventory count
        public bool IsAvailable { get; set; } = true;   // Availability flag
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int CategoryId { get; set; }
        public Category Category { get; set; }          // Navigation property

        // One-to-many relationship (a product can have multiple images)
        public ICollection<ImageEntity>? Images { get; set; }


    }
}
