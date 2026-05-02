using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }                     // Primary Key
        public string Name { get; set; }                // Category name
        public string Description { get; set; }         // Optional description
        public bool IsActive { get; set; } = true;      // Active flag
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        // One-to-one relationship
        // One-to-one relationship (Category has one Image)
        public ImageEntity? Image { get; set; }


        // Navigation property
        public ICollection<Product>? Products { get; set; }

        // Ingredients belonging to this category (e.g. "Cake" → Cocoa, Cream)
        public ICollection<Ingredient>? Ingredients { get; set; }
    }
}
