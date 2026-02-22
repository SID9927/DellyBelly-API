using DellyBelly.Application.DTOs;
using DellyBelly.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetBestSellersAsync(int count);
        Task<IEnumerable<Product>> GetRecommendedAsync(int count);

        /// <summary>
        /// Returns a paginated, filterable, sortable page of products.
        /// </summary>
        Task<PagedResult<Product>> GetPagedAsync(
            int page,
            int pageSize,
            string? category = null,
            string? search = null,
            string? sortBy = null,
            decimal? minPrice = null,
            decimal? maxPrice = null);
    }
}
