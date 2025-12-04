using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DellyBelly.Application.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext _context;

        public GalleryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gallery>> GetAllAsync()
        {
            return await _context.Galleries
                .Include(g => g.Images)
                .ToListAsync();
        }

        public async Task<Gallery?> GetByIdAsync(int id)
        {
            return await _context.Galleries
                .Include(g => g.Images)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gallery> CreateAsync(Gallery gallery)
        {
            // Handle empty/invalid image objects from client
            if (gallery.Images != null && gallery.Images.Any())
            {
                var validImages = gallery.Images
                    .Where(i => !string.IsNullOrEmpty(i.FileName) && i.Data != null && i.Data.Length > 0)
                    .ToList();

                gallery.Images = validImages.Any() ? validImages : null;
            }

            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();
            return gallery;
        }

        public async Task<Gallery> UpdateAsync(Gallery gallery)
        {
            _context.Galleries.Update(gallery);
            await _context.SaveChangesAsync();
            return gallery;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return false;

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
