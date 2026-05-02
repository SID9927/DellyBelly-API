using DellyBelly.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DellyBelly.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetUserWishlistAsync(int userId);
        Task<bool> ToggleWishlistAsync(int userId, int productId);
        Task<bool> IsInWishlistAsync(int userId, int productId);
        Task<bool> ClearWishlistAsync(int userId);
    }
}
