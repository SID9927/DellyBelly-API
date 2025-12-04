using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Domain.Entities
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One-to-many relationship with ImageEntity
        public ICollection<ImageEntity>? Images { get; set; }
    }
}
