using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DellyBelly.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Feedback Categories ---

        public async Task<IEnumerable<FeedbackCategory>> GetAllCategoriesAsync()
        {
            return await _context.FeedbackCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<FeedbackCategory?> GetCategoryByIdAsync(int id)
        {
            return await _context.FeedbackCategories.FindAsync(id);
        }

        public async Task<FeedbackCategory> CreateCategoryAsync(FeedbackCategory category)
        {
            _context.FeedbackCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<FeedbackCategory> UpdateCategoryAsync(FeedbackCategory category)
        {
            _context.FeedbackCategories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.FeedbackCategories.FindAsync(id);
            if (category == null) return false;

            _context.FeedbackCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- Feedbacks ---

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(bool? approvedOnly = null)
        {
            var query = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Category)
                .AsNoTracking();

            if (approvedOnly.HasValue)
            {
                query = query.Where(f => f.IsApproved == approvedOnly.Value);
            }

            return await query
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int id)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Feedback> SubmitFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> ApproveFeedbackAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            feedback.IsApproved = !feedback.IsApproved; // Toggle visibility
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return false;

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
