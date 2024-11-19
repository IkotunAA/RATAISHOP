using RATAISHOP.Entities;

namespace RATAISHOP.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int buyerId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task ClearCartByBuyerIdAsync(int BuyerId);
    }

}
