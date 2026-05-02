using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DellyBelly.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetUserWishlistAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .ThenInclude(p => p.Images)
                .Include(w => w.Product.Category)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.AddedAt)
                .ToListAsync();
        }

        public async Task<bool> ToggleWishlistAsync(int userId, int productId)
        {
            var existing = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existing != null)
            {
                _context.Wishlists.Remove(existing);
                await _context.SaveChangesAsync();
                return false; // Removed
            }
            else
            {
                var newItem = new Wishlist
                {
                    UserId = userId,
                    ProductId = productId,
                    AddedAt = DateTimeHelper.GetIndianTime()
                };
                _context.Wishlists.Add(newItem);
                await _context.SaveChangesAsync();
                return true; // Added
            }
        }

        public async Task<bool> IsInWishlistAsync(int userId, int productId)
        {
            return await _context.Wishlists.AnyAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task<bool> ClearWishlistAsync(int userId)
        {
            var items = _context.Wishlists.Where(w => w.UserId == userId);
            if (!items.Any()) return false;
            
            _context.Wishlists.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
