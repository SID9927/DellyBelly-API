using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/customer/addresses")]
    [Authorize]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomerAddressController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int GetCustomerId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim, out int id)) return id;
            return 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var customerId = GetCustomerId();
            if (customerId == 0) return Unauthorized();

            var addresses = await _db.CustomerAddresses
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .Select(a => new CustomerAddressResponseDto(a.Id, a.Type, a.AddressText, a.IsDefault))
                .ToListAsync();

            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] CreateCustomerAddressDto dto)
        {
            var customerId = GetCustomerId();
            if (customerId == 0) return Unauthorized();

            // If this is the first address, or IsDefault is true, make others not default
            var existingCount = await _db.CustomerAddresses.CountAsync(a => a.CustomerId == customerId);
            bool isDefault = existingCount == 0 || dto.IsDefault;

            if (isDefault && existingCount > 0)
            {
                var currentDefaults = await _db.CustomerAddresses
                    .Where(a => a.CustomerId == customerId && a.IsDefault)
                    .ToListAsync();
                foreach (var addr in currentDefaults) addr.IsDefault = false;
            }

            var address = new CustomerAddress
            {
                CustomerId = customerId,
                Type = dto.Type,
                AddressText = dto.AddressText,
                IsDefault = isDefault,
                CreatedAt = DateTime.UtcNow
            };

            _db.CustomerAddresses.Add(address);
            await _db.SaveChangesAsync();

            return Ok(new CustomerAddressResponseDto(address.Id, address.Type, address.AddressText, address.IsDefault));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var customerId = GetCustomerId();
            if (customerId == 0) return Unauthorized();

            var address = await _db.CustomerAddresses.FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == customerId);
            if (address == null) return NotFound();

            _db.CustomerAddresses.Remove(address);
            await _db.SaveChangesAsync();

            // If we deleted the default, make another one default
            if (address.IsDefault)
            {
                var nextAddress = await _db.CustomerAddresses.FirstOrDefaultAsync(a => a.CustomerId == customerId);
                if (nextAddress != null)
                {
                    nextAddress.IsDefault = true;
                    await _db.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Address deleted successfully." });
        }

        [HttpPut("{id}/default")]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var customerId = GetCustomerId();
            if (customerId == 0) return Unauthorized();

            var newDefault = await _db.CustomerAddresses.FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == customerId);
            if (newDefault == null) return NotFound();

            var currentDefaults = await _db.CustomerAddresses
                    .Where(a => a.CustomerId == customerId && a.IsDefault)
                    .ToListAsync();
            foreach (var addr in currentDefaults) addr.IsDefault = false;

            newDefault.IsDefault = true;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Default address updated." });
        }
    }
}
