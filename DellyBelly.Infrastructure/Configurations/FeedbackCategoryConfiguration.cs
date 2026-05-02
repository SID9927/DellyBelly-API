using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DellyBelly.Domain.Entities;

namespace DellyBelly.Infrastructure.Configurations
{
    public class FeedbackCategoryConfiguration : IEntityTypeConfiguration<FeedbackCategory>
    {
        public void Configure(EntityTypeBuilder<FeedbackCategory> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(c => c.Description)
                   .HasMaxLength(500);
            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);
            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
