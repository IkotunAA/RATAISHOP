using RATAISHOP.Entities;

namespace RATAISHOP.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int buyerId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> GetOrderByReferenceAsync(string reference);
    }


}
