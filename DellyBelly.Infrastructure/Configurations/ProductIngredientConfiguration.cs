using DellyBelly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DellyBelly.Infrastructure.Configurations
{
    public class ProductIngredientConfiguration : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            // Composite primary key — no separate Id column needed
            builder.HasKey(pi => new { pi.ProductId, pi.IngredientId });

            builder.HasOne(pi => pi.Product)
                   .WithMany(p => p.ProductIngredients)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade); // Delete product → delete its ingredient links

            builder.HasOne(pi => pi.Ingredient)
                   .WithMany(i => i.ProductIngredients)
                   .HasForeignKey(pi => pi.IngredientId)
                   .OnDelete(DeleteBehavior.NoAction); // Keep ingredients even if product is deleted
        }
    }
}
