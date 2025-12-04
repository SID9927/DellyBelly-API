using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(c => c.Email)
                   .IsUnique();

            builder.Property(c => c.PhoneNumber)
                   .HasMaxLength(15);

            builder.Property(c => c.Address)
                   .HasMaxLength(255);

            builder.Property(c => c.TotalSpent)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0);

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }

}
