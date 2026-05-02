using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DellyBelly.Infrastructure.Configurations
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(i => i.IsAllergen)
                   .HasDefaultValue(false);

            builder.Property(i => i.IsActive)
                   .HasDefaultValue(true);

            builder.Property(i => i.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Each ingredient belongs to one category
            builder.HasOne(i => i.Category)
                   .WithMany(c => c.Ingredients)
                   .HasForeignKey(i => i.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete category → delete its ingredients

            // Unique: same ingredient name cannot appear twice in the same category
            builder.HasIndex(i => new { i.CategoryId, i.Name }).IsUnique();
        }
    }
}
