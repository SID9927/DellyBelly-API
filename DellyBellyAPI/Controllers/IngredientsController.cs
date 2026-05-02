using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DellyBellyAPI.Controllers
{
    /// <summary>
    /// Manages ingredients for bakery products.
    /// Ingredients are linked to Categories so the admin product form
    /// can show only relevant ingredients when a category is selected.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        /// <summary>GET /api/ingredients — all active ingredients with their category</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingredients = await _ingredientService.GetAllAsync();
            return Ok(ingredients.Select(i => new
            {
                i.Id,
                i.Name,
                i.IsAllergen,
                i.IsActive,
                i.CategoryId,
                CategoryName = i.Category?.Name,
            }));
        }

        /// <summary>
        /// GET /api/ingredients/by-category/{categoryId}
        /// Used by the ProductForm to load relevant ingredients when a category is selected.
        /// </summary>
        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var ingredients = await _ingredientService.GetByCategoryAsync(categoryId);
            return Ok(ingredients.Select(i => new
            {
                i.Id,
                i.Name,
                i.IsAllergen,
                i.CategoryId,
            }));
        }

        /// <summary>GET /api/ingredients/{id}</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();
            return Ok(new
            {
                ingredient.Id,
                ingredient.Name,
                ingredient.IsAllergen,
                ingredient.IsActive,
                ingredient.CategoryId,
                CategoryName = ingredient.Category?.Name,
            });
        }

        /// <summary>POST /api/ingredients</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIngredientDto dto)
        {
            var ingredient = new Ingredient
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsAllergen = dto.IsAllergen,
                IsActive = dto.IsActive,
            };

            var created = await _ingredientService.CreateAsync(ingredient);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                created.Id,
                created.Name,
                created.IsAllergen,
                created.IsActive,
                created.CategoryId,
            });
        }

        /// <summary>PUT /api/ingredients/{id}</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateIngredientDto dto)
        {
            var existing = await _ingredientService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.CategoryId = dto.CategoryId;
            existing.IsAllergen = dto.IsAllergen;
            existing.IsActive = dto.IsActive;
            existing.Category = null; // Clear nav property so FK takes effect

            var updated = await _ingredientService.UpdateAsync(existing);
            return Ok(new { updated.Id, updated.Name, updated.IsAllergen, updated.IsActive, updated.CategoryId });
        }

        /// <summary>DELETE /api/ingredients/{id}</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ingredientService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
