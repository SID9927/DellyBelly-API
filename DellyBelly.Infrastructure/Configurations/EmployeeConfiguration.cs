using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.PasswordHash)
                   .IsRequired();

            builder.Property(e => e.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
