using RATAISHOP.Models;

namespace RATAISHOP.Services.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<OrderDto>> PlaceOrderAsync(int userId);
        Task<BaseResponse<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId);
        Task<BaseResponse<OrderDto>> GetOrderByIdAsync(int orderId);
        //Task<OrderResponse> GetByIdAsync(int id);
        //Task<OrderResponse> GetAllAsync();
        //Task<OrderResponse> CreateAsync(OrderDto orderDto);
        //Task<OrderResponse> UpdateAsync(OrderDto orderDto);
        //Task<OrderResponse> PlaceOrderAsync(int buyerId, IEnumerable<int> cartItemIds, string paymentMethod, decimal totalAmount);
        //Task<BaseResponse> DeleteAsync(int id);
    }

}
