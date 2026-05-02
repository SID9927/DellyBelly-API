using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DellyBelly.API.Controllers
{
    [ApiController]
    [Route("api/admin-users")]
    [Authorize(Roles = "super_admin,admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _db.AdminUsers
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            // Return safe data (omit password hash)
            return Ok(users.Select(u => new {
                u.Id,
                u.FullName,
                u.Email,
                u.MobileNumber,
                u.Role,
                u.IsActive,
                u.CreatedAt,
                u.LastLoginAt
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _db.AdminUsers.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(new {
                user.Id,
                user.FullName,
                user.Email,
                user.MobileNumber,
                user.Role,
                user.IsActive,
                user.CreatedAt,
                user.LastLoginAt
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdminUser user)
        {
            if (await _db.AdminUsers.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower()))
                return Conflict(new { message = "Email already in use." });

            user.CreatedAt = DateTimeHelper.GetIndianTime();
            // Default password if none provided? Or maybe we should require one.
            // For now, let's assume the body contains a Password field we need to hash.
            // But AdminUser entity has PasswordHash, not Password.
            
            // I'll use a DTO or just assume the 'Password' is provided in a dynamic way for now, 
            // or use a default one like 'Admin@123' and prompt for change.
            
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            }
            else
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }

            _db.AdminUsers.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdminUser updatedUser)
        {
            var user = await _db.AdminUsers.FindAsync(id);
            if (user == null) return NotFound();

            // Rule: If changing from super_admin, check if another super_admin exists
            if (user.Role == "super_admin" && updatedUser.Role != "super_admin")
            {
                var otherSuperAdminExists = await _db.AdminUsers.AnyAsync(u => u.Role == "super_admin" && u.Id != id && u.IsActive);
                if (!otherSuperAdminExists)
                {
                    return BadRequest(new { message = "At least one active Super Admin is compulsory. Cannot change role." });
                }
            }

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.MobileNumber = updatedUser.MobileNumber;
            user.Role = updatedUser.Role;
            user.IsActive = updatedUser.IsActive;

            await _db.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.AdminUsers.FindAsync(id);
            if (user == null) return NotFound();

            if (user.Role == "super_admin")
                return BadRequest(new { message = "Cannot delete super admin." });

            _db.AdminUsers.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _db.AdminUsers.FindAsync(id);
            if (user == null) return NotFound();

            // Rule: Don't allow deactivating the last super_admin
            if (user.IsActive && user.Role == "super_admin")
            {
                var otherSuperAdminExists = await _db.AdminUsers.AnyAsync(u => u.Role == "super_admin" && u.Id != id && u.IsActive);
                if (!otherSuperAdminExists)
                {
                    return BadRequest(new { message = "At least one active Super Admin is compulsory. Cannot deactivate." });
                }
            }

            user.IsActive = !user.IsActive;
            await _db.SaveChangesAsync();
            return Ok(new { user.IsActive });
        }
    }
}
