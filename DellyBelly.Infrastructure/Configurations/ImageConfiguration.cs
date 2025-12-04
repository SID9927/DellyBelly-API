using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<ImageEntity>
    {
        public void Configure(EntityTypeBuilder<ImageEntity> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.FileName).IsRequired().HasMaxLength(255);
            builder.Property(i => i.ContentType).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Data).IsRequired();
            builder.Property(i => i.Source).IsRequired().HasMaxLength(50);
        }
    }

}
