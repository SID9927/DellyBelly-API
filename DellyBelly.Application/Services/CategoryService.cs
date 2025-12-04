using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Image)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            // Handle empty/invalid image object from client
            if (category.Image != null)
            {
                if (string.IsNullOrEmpty(category.Image.FileName) || category.Image.Data == null || category.Image.Data.Length == 0)
                {
                    category.Image = null;
                }
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Image)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return false;

            // Remove the associated image if it exists
            if (category.Image != null)
            {
                _context.Images.Remove(category.Image);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
