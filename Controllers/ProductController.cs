using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Models;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Authorize(Roles = "Seller")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            var sellerId = int.Parse(User.FindFirst("Id").Value);
            var response = await _productService.CreateProductAsync(productDto, sellerId);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            var sellerId = int.Parse(User.FindFirst("Id").Value);
            productDto.Id = id;
            var response = await _productService.UpdateProductAsync(id, productDto, sellerId);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllProductAsync();
            return Ok(response.Data);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Status == false)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            var response = await _productService.SearchProductsAsync(searchTerm);
            return Ok(response.Data);
        }
    }

}
