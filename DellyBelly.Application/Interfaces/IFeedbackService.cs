using DellyBelly.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DellyBelly.Application.Interfaces
{
    public interface IFeedbackService
    {
        // Feedback Categories
        Task<IEnumerable<FeedbackCategory>> GetAllCategoriesAsync();
        Task<FeedbackCategory?> GetCategoryByIdAsync(int id);
        Task<FeedbackCategory> CreateCategoryAsync(FeedbackCategory category);
        Task<FeedbackCategory> UpdateCategoryAsync(FeedbackCategory category);
        Task<bool> DeleteCategoryAsync(int id);

        // Feedbacks
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(bool? approvedOnly = null);
        Task<Feedback?> GetFeedbackByIdAsync(int id);
        Task<Feedback> SubmitFeedbackAsync(Feedback feedback);
        Task<bool> ApproveFeedbackAsync(int id);
        Task<bool> DeleteFeedbackAsync(int id);
    }
}
