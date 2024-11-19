using AutoMapper;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.PaymentModel;
using RATAISHOP.PaymentServices.Interfaces;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IWalletService walletService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _walletService = walletService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<OrderDto>> PlaceOrderAsync(int userId)
        {
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);
            if (!cartItems.Any())
            {
                return new BaseResponse<OrderDto>
                {
                    Status = false,
                    Message = "No items in cart."
                };
            }

            var order = new Order
            {
                BuyerId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList(),
                TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            await _orderRepository.CreateOrderAsync(order);

            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                product.Quantity -= item.Quantity;
                await _productRepository.UpdateProductAsync(product);

                // Update the seller's wallet
                //await _walletService.UpdateWalletAsync(product.SellerId, item.Quantity * item.UnitPrice);
            }

            await _cartRepository.ClearCartByBuyerIdAsync(userId);
            _unitOfWork.SaveChanges();
            var orderDto = _mapper.Map<OrderDto>(order);

            return new BaseResponse<OrderDto>//(true, "Order placed successfully.", orderDto);
            {
                Status = true,
                Message = "Order placed successfully.",
                Data = orderDto
            };
        }

        public async Task<BaseResponse<IEnumerable<OrderDto>>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

            return new BaseResponse<IEnumerable<OrderDto>>//(true, "Orders retrieved successfully.", orderDtos);
            {
                Status = true,
                Message = "Orders retrieved successfully.",
                Data = orderDtos
            };
        }

        public async Task<BaseResponse<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return new BaseResponse<OrderDto>//(false, "Order not found.");
                {
                    Status = false,
                    Message = "Order not found."
                };
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return new BaseResponse<OrderDto>//(true, "Order retrieved successfully.", orderDto);
            {
                Status = true,
                Message = "Orders retrieved successfully.",
                Data = orderDto
            };
        }

        //        public async Task<OrderResponse> GetByIdAsync(int id)
        //        {
        //            var order = await _orderRepository.GetByIdAsync(id);
        //            if (order == null)
        //            {
        //                return new OrderResponse { Status = false, Message = "Order not found." };
        //            }

        //            return new OrderResponse { Status = true, Data = _mapper.Map<OrderDto>(order) };
        //        }

        //        public async Task<BaseResponse> GetAllAsync()
        //        {
        //            var orders = await _orderRepository.GetAllAsync();
        //            return new BaseResponse { Status = true, Message = "Orders retrieved successfully.", /*Data = _mapper.Map<IEnumerable<OrderDto>>(orders)*/ };
        //        }

        //        public async Task<BaseResponse> CreateAsync(OrderDto orderDto)
        //        {
        //            var order = _mapper.Map<Order>(orderDto);
        //            await _orderRepository.AddAsync(order);
        //            return new BaseResponse { Status = true, Message = "Order created successfully." };
        //        }

        //        public async Task<BaseResponse> UpdateAsync(OrderDto orderDto)
        //        {`
        //            var order = _mapper.Map<Order>(orderDto);
        //            await _orderRepository.UpdateAsync(order);
        //            return new BaseResponse { Status = true, Message = "Order updated successfully." };
        //        }

        //        public async Task<BaseResponse> DeleteAsync(int id)
        //        {
        //            await _orderRepository.DeleteAsync(id);
        //            return new BaseResponse { Status = true, Message = "Order deleted successfully." };
        //        }

        //        Task<OrderResponse> IOrderService.GetAllAsync()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        Task<OrderResponse> IOrderService.CreateAsync(OrderDto orderDto)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        Task<OrderResponse> IOrderService.UpdateAsync(OrderDto orderDto)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public Task<OrderResponse> PlaceOrderAsync(int buyerId, IEnumerable<int> cartItemIds, string paymentMethod, decimal totalAmount)
        //        {
        //            var PlaceOrder = _orderRepository.GetByIdAsync(buyerId);
        //            OrderResponse paymentResponse;
        //            var PaymentModel =  StripePaymentModel

        //            if (paymentMethod.ToLower() == "stripe")
        //            {

        //                paymentResponse = _iStripePaymentService.ProcessPaymentAsync(paymentmodel);
        //            }
        //            elseif(paymentMethod.ToLower() == "paystack")
        //        {
        //                paymentResponse = await _paymentService.ProcessPaystackPaymentAsync(totalAmount, "buyer_email@example.com", "unique_reference");
        //            }
        //        else
        //            {
        //                returnnew BaseResponse { Status = false, Message = "Invalid payment method" };
        //            }

        //            if (!paymentResponse.Status)
        //            {
        //                returnnew BaseResponse { Status = false, Message = "Payment failed", Data = paymentResponse.Message };
        //            }

        //            returnnew BaseResponse { Status = true, Message = "Order placed successfully", Data = order };
        //        }

        //    }
    }

}
