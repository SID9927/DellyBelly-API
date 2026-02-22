using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } = "image/webp";
        public byte[] Data { get; set; }
        public DateTime UploadedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        // Discriminator / Source
        public string Source { get; set; } // "Category", "Product", "Gallery"

        // Foreign Keys
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        
        // Navigation Properties
        public Category? Category { get; set; }
    }

}
