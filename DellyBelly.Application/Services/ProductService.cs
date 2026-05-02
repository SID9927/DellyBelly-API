using DellyBelly.Application.DTOs;
using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DellyBelly.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .ToListAsync(); // No .Include(Images) — we load metadata only below

            await AttachImageMetaAsync(products);
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductIngredients!)
                    .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            // Handle empty/invalid image objects from client
            if (product.Images != null && product.Images.Any())
            {
                var validImages = product.Images
                    .Where(i => !string.IsNullOrEmpty(i.FileName) && i.Data != null && i.Data.Length > 0)
                    .ToList();
                
                product.Images = validImages.Any() ? validImages : null;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetBestSellersAsync(int count)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsBestSeller && p.IsAvailable)
                .Include(p => p.Category)
                .Take(count)
                .ToListAsync();

            await AttachImageMetaAsync(products);
            return products;
        }

        public async Task<IEnumerable<Product>> GetRecommendedAsync(int count)
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(p => p.IsRecommended && p.IsAvailable)
                .Include(p => p.Category)
                .Take(count)
                .ToListAsync();

            await AttachImageMetaAsync(products);
            return products;
        }

        /// <summary>
        /// Loads ONLY image Id/ContentType/FileName for the given products.
        /// The heavy Data column is never fetched from the database.
        /// Frontend constructs image URLs as /api/products/{id}/photo/{imageId}.
        /// </summary>
        private async Task AttachImageMetaAsync(IEnumerable<Product> products)
        {
            var productList = products.ToList();
            if (!productList.Any()) return;

            var ids = productList.Select(p => p.Id).ToList();

            // SELECT Id, ProductId, ContentType, FileName — NO Data column
            var metas = await _context.Images
                .AsNoTracking()
                .Where(i => i.ProductId.HasValue && ids.Contains(i.ProductId.Value))
                .Select(i => new { i.Id, i.ProductId, i.ContentType, i.FileName })
                .ToListAsync();

            var lookup = metas.ToLookup(i => i.ProductId);

            foreach (var p in productList)
            {
                p.Images = lookup[p.Id]
                    .Select(i => new ImageEntity
                    {
                        Id          = i.Id,
                        ProductId   = i.ProductId,
                        ContentType = i.ContentType,
                        FileName    = i.FileName
                        // Data intentionally NOT set — stays null
                    }).ToList();
            }
        }

        public async Task<PagedResult<Product>> GetPagedAsync(
            int page,
            int pageSize,
            string? category = null,
            string? search = null,
            string? sortBy = null,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            // Clamp values to safe minimums
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            // Build base query — Images NOT included here (loaded via AttachImageMetaAsync below)
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsAvailable)
                .AsQueryable();

            // Category filter (case-insensitive match on name)
            if (!string.IsNullOrWhiteSpace(category) && category.ToLower() != "all")
                query = query.Where(p => p.Category != null &&
                    p.Category.Name.ToLower() == category.ToLower());

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            // Price range filter
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            // Sorting
            query = sortBy switch
            {
                "low-high"  => query.OrderBy(p => p.Price),
                "high-low"  => query.OrderByDescending(p => p.Price),
                _           => query.OrderBy(p => p.Name)
            };

            // Get total BEFORE pagination (single COUNT query)
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            await AttachImageMetaAsync(items);

            return new PagedResult<Product>
            {
                Items      = items,
                Page       = page,
                PageSize   = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
