using System.ComponentModel.DataAnnotations;

namespace DellyBellyAPI.DTOs
{
    public class CreateFeedbackDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
    }

    public class CreateFeedbackCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
