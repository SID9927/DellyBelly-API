using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public CategoryService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            const string cacheKey = "all_categories";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<Category> categories))
            {
                // Step 1: load categories (no images yet)
                var list = await _context.Categories
                    .AsNoTracking()
                    .Where(c => c.IsActive)
                    .ToListAsync();

                // Step 2: load ONLY image Id/ContentType/FileName — Data column never touched
                var catIds = list.Select(c => c.Id).ToList();
                var imageMetas = await _context.Images
                    .AsNoTracking()
                    .Where(i => i.CategoryId.HasValue && catIds.Contains(i.CategoryId.Value) && i.Source == "Category")
                    .Select(i => new { i.Id, i.CategoryId, i.ContentType, i.FileName })
                    .ToListAsync();

                var imgLookup = imageMetas.ToDictionary(i => i.CategoryId);
                foreach (var cat in list)
                {
                    if (imgLookup.TryGetValue(cat.Id, out var img))
                    {
                        cat.Image = new ImageEntity
                        {
                            Id          = img.Id,
                            CategoryId  = img.CategoryId,
                            ContentType = img.ContentType,
                            FileName    = img.FileName
                            // Data intentionally NOT loaded
                        };
                    }
                }

                categories = list;

                _cache.Set(cacheKey, categories, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            }
            return categories;
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
            _cache.Remove("all_categories"); // Invalidate cache
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            _cache.Remove("all_categories"); // Invalidate cache
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
            _cache.Remove("all_categories"); // Invalidate cache
            return true;
        }
    }

}
