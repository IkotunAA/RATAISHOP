using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Models;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Authorize(Roles = "Buyer")]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = int.Parse(User.FindFirst("Id").Value);
            var response = await _cartService.GetCartItemsByUserIdAsync(userId);
            return Ok(response.Data);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst("Id").Value);
            var response = await _cartService.AddtoCartAsync(userId, productId, quantity);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = int.Parse(User.FindFirst("Id").Value);
            var response = await _cartService.RemovefromCartAsync(userId, cartItemId);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirst("Id").Value);
            var response = await _cartService.ClearCartAsync(userId);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }
    }

}


