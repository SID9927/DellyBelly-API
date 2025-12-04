using DellyBelly.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DellyBelly.Application.Interfaces
{
    public interface IGalleryService
    {
        Task<IEnumerable<Gallery>> GetAllAsync();
        Task<Gallery?> GetByIdAsync(int id);
        Task<Gallery> CreateAsync(Gallery gallery);
        Task<Gallery> UpdateAsync(Gallery gallery);
        Task<bool> DeleteAsync(int id);
    }
}
