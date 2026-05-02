using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DellyBellyAPI.DTOs
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsBestSeller { get; set; } = false;
        public bool IsRecommended { get; set; } = false;

        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// IDs of ingredients selected for this product.
        /// Optional — products can have zero ingredients.
        /// </summary>
        public List<int>? IngredientIds { get; set; }
    }
}
