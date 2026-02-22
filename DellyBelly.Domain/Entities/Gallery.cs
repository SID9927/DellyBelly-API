using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Gallery
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;

        public string FileName { get; set; }
        public string ContentType { get; set; } = "image/webp";
        public byte[] Data { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
