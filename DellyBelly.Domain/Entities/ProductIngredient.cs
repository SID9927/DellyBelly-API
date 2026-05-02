namespace DellyBelly.Domain.Entities
{
    /// <summary>
    /// Explicit many-to-many join entity between Product and Ingredient.
    /// Using an explicit join class (instead of EF implicit) keeps the door open
    /// to add future fields like "Quantity", "IsOptional", etc.
    /// </summary>
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
