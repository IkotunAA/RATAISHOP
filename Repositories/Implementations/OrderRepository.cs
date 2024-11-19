using Microsoft.EntityFrameworkCore;
using RATAISHOP.Context;
using RATAISHOP.Entities;
using RATAISHOP.Repositories.Interfaces;

namespace RATAISHOP.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RataiDbContext _context;

        public OrderRepository(RataiDbContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int buyerId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == buyerId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public  async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return order;
            
        }
        public async Task<Order?> GetOrderByReferenceAsync(string reference)
        {
            return await _context.Orders.SingleOrDefaultAsync(o => o.PaymentReference == reference);
        }
    }

}
