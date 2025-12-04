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
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Discriminator / Source
        public string Source { get; set; } // "Category", "Product", "Gallery"

        // Foreign Keys
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public int? GalleryId { get; set; }

        // Navigation Properties
        public Category? Category { get; set; }
        public Gallery? Gallery { get; set; }
    }

}
