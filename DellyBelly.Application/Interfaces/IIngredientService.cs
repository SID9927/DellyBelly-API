using DellyBelly.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DellyBelly.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<IEnumerable<Ingredient>> GetByCategoryAsync(int categoryId);
        Task<Ingredient?> GetByIdAsync(int id);
        Task<Ingredient> CreateAsync(Ingredient ingredient);
        Task<Ingredient> UpdateAsync(Ingredient ingredient);
        Task<bool> DeleteAsync(int id);
    }
}
