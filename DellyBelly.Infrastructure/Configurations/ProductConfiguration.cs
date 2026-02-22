using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(p => p.Description)
                   .HasMaxLength(500);
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Stock)
                   .HasDefaultValue(0);
            builder.Property(p => p.IsAvailable)
                   .HasDefaultValue(true);
            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.IsBestSeller)
                   .HasDefaultValue(false);
            builder.Property(p => p.IsRecommended)
                   .HasDefaultValue(false);
            // One-to-many relationship with ImageEntity
            builder.HasMany(p => p.Images)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
