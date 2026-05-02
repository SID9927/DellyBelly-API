using System.ComponentModel.DataAnnotations;

namespace DellyBellyAPI.DTOs
{
    public class CreateIngredientDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsAllergen { get; set; } = false;

        public bool IsActive { get; set; } = true;
    }
}
