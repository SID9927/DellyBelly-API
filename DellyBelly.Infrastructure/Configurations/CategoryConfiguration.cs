using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DellyBelly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c=> c.Id);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(c => c.Description)
                   .HasMaxLength(500);
            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);
            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            // One-to-one relationship with ImageEntity
            builder.HasOne(c => c.Image)
                   .WithOne(i => i.Category)
                   .HasForeignKey<ImageEntity>(i => i.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
