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
            builder.Property(g => g.IsActive).HasDefaultValue(true);
            builder.Property(g => g.UploadedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
