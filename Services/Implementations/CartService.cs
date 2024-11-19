using AutoMapper;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.Repositories.Implementations;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository,IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItemResponse> GetCartItemsByUserIdAsync(int buyerId)
        {
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(buyerId);
            var cartItemDtos = (CartItemDto)_mapper.Map<IEnumerable<CartItemDto>>(cartItems);
            return new CartItemResponse { Status = true, Message = "Cart items retrieved successfully.", Data = cartItemDtos };
        }

        public async Task<CartItemResponse> AddtoCartAsync(int buyerId, int productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.Quantity < quantity)
            {
                return new CartItemResponse
                { 
                    Status = false, 
                    Message = "Product is not available in the desired quantity."
                };
            }
            var cartItem = new CartItem
            {
                BuyerId = buyerId,
                ProductId = productId,
                Quantity = quantity
            };
            await _cartRepository.AddCartItemAsync(cartItem);
            _unitOfWork.SaveChanges();
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);
            return new CartItemResponse { Status = true, Message = "Item added to cart successfully.", Data = cartItemDto};
            
        }

        public async Task<CartItemResponse> RemovefromCartAsync(int buyerId, int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null || cartItem.BuyerId != buyerId)
            {
                return new CartItemResponse
                {
                    Status = false,
                    Message = "Cart item not found or not owned by user."
                };
            }
            _cartRepository.DeleteCartItemAsync(cartItem.Id);
            _unitOfWork.SaveChanges();
            return new CartItemResponse { Status = true, Message = "Item removed from cart successfully." };
        }

        public async Task<BaseResponse<bool>> ClearCartAsync(int userId)
        {
            var cart = _cartRepository.GetCartItemsByUserIdAsync(userId);
            if (cart == null)
            {
                return new BaseResponse<bool>()
                {
                    Status = false,
                    Message = "Cart is empty"
                };
            }
            await _cartRepository.ClearCartByBuyerIdAsync(userId);
            _unitOfWork.SaveChanges();
            return new BaseResponse<bool>
            {
                Status = true,
                Message = "Cart cleared successfully."
            };
        }
    }

}
