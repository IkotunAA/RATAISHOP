using Microsoft.EntityFrameworkCore;
using RATAISHOP.Context;
using RATAISHOP.Entities;
using RATAISHOP.Repositories.Interfaces;

namespace RATAISHOP.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly RataiDbContext _context;

        public CartRepository(RataiDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int buyerId)
        {
            return await _context.CartItems
                .Where(c => c.BuyerId == buyerId)
                .Include(c => c.Product)
                .ToListAsync();
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }

        public async Task ClearCartByBuyerIdAsync(int BuyerId)
        {
            var cartItems = await _context.CartItems
            .Where(ci => ci.BuyerId == BuyerId)
            .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
