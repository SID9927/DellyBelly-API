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
                .ToListAsync();
        }

        public async Task<Gallery?> GetByIdAsync(int id)
        {
            return await _context.Galleries
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Gallery> CreateAsync(Gallery gallery)
        {
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
