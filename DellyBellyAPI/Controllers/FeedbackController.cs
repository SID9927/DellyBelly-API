using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // --- Categories ---

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _feedbackService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost("categories")]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateFeedbackCategoryDto dto)
        {
            var category = new FeedbackCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
            var created = await _feedbackService.CreateCategoryAsync(category);
            return Ok(created);
        }

        [HttpPut("categories/{id}")]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateFeedbackCategoryDto dto)
        {
            var existing = await _feedbackService.GetCategoryByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;

            await _feedbackService.UpdateCategoryAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("categories/{id}")]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _feedbackService.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // --- Feedbacks ---

        [HttpGet]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> GetAllFeedbacks([FromQuery] bool? approvedOnly = null)
        {
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync(approvedOnly);
            return Ok(feedbacks);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitFeedback([FromBody] CreateFeedbackDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var feedback = new Feedback
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Subject = dto.Subject,
                Comment = dto.Comment,
                Rating = dto.Rating,
                IsApproved = false,
                SubmittedAt = DateTimeHelper.GetIndianTime()
            };

            var created = await _feedbackService.SubmitFeedbackAsync(feedback);
            return Ok(created);
        }

        [HttpPatch("{id}/approve")]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> ApproveFeedback(int id)
        {
            var result = await _feedbackService.ApproveFeedbackAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "super_admin,admin")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var result = await _feedbackService.DeleteFeedbackAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
