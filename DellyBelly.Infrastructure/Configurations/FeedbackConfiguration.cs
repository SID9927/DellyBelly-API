using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DellyBelly.Domain.Entities;

namespace DellyBelly.Infrastructure.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Subject)
                   .IsRequired()
                   .HasMaxLength(255);
            builder.Property(f => f.Comment)
                   .IsRequired();
            builder.Property(f => f.Rating)
                   .IsRequired();
            builder.Property(f => f.IsApproved)
                   .HasDefaultValue(false);
            builder.Property(f => f.SubmittedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(f => f.User)
                   .WithMany()
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Category)
                   .WithMany(c => c.Feedbacks)
                   .HasForeignKey(f => f.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
