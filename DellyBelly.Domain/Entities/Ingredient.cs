using System;
using System.Collections.Generic;

namespace DellyBelly.Domain.Entities
{
    /// <summary>
    /// Represents a single ingredient (e.g. "Premium Cocoa", "Farm Fresh Cream").
    /// Each ingredient belongs to a Category so the ProductForm can filter
    /// and show only relevant ingredients when a category is selected.
    /// </summary>
    public class Ingredient
    {
        public int Id { get; set; }

        /// <summary>Display name shown as a pill on the Product Details page.</summary>
        public string Name { get; set; }

        /// <summary>
        /// When true, a "May contain allergens" warning is shown on the product page.
        /// </summary>
        public bool IsAllergen { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        // ── Relationships ────────────────────────────────────────────────────────

        /// <summary>The category this ingredient belongs to (e.g. "Cake" → Cocoa).</summary>
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        /// <summary>Products that include this ingredient (many-to-many via join table).</summary>
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
    }
}
