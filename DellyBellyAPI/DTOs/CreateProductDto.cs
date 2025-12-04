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

        [Required]
        public int CategoryId { get; set; }
    }
}
