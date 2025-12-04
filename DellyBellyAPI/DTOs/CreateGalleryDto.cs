using System.ComponentModel.DataAnnotations;

namespace DellyBellyAPI.DTOs
{
    public class CreateGalleryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
