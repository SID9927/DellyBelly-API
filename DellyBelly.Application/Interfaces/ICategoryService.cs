using DellyBelly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);

    }
}
