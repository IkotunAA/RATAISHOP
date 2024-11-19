using RATAISHOP.Models;

namespace RATAISHOP.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartItemResponse> GetCartItemsByUserIdAsync(int buyerId);
        Task<CartItemResponse> AddtoCartAsync(int buyerId, int productId, int quantity);
        Task<CartItemResponse> RemovefromCartAsync(int buyerId, int cartItemId);
        Task<BaseResponse<bool>> ClearCartAsync(int userId);
    }

}
