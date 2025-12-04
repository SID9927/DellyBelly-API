using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class GalleryConfiguration : IEntityTypeConfiguration<Gallery>
    {
        public void Configure(EntityTypeBuilder<Gallery> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Description).HasMaxLength(500);
            builder.Property(g => g.IsActive).HasDefaultValue(true);
            builder.Property(g => g.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // One-to-many relationship with ImageEntity
            builder.HasMany(g => g.Images)
                   .WithOne(i => i.Gallery)
                   .HasForeignKey(i => i.GalleryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
