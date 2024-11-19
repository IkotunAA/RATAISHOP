using AutoMapper;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.Repositories.Implementations;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return new ProductResponse { Status = false, Message = "Product not found." };
            }

            return new ProductResponse 
            { Status = true, 
              Data = _mapper.Map<ProductDto>(product),
              Product = new Product
              {
                  Id = product.Id,
                  Name = product.Name,
                  Seller = product.Seller,
                  ImageUrl = product.ImageUrl,
                  Description = product.Description,
                  Price = product.Price,
                  Quantity = product.Quantity
              }
            };
        }

        public async Task<ProductResponse> GetAllProductAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return new ProductResponse 
            { Status = true, 
              Message = "Products retrieved successfully.",
              Data = _mapper.Map<ProductDto>(products)
            };
        }

        public async Task<ProductResponse> CreateProductAsync(ProductDto productDto, int sellerId)
        {
            var product = _mapper.Map<Product>(productDto);
            product.SellerId = sellerId;
            await _productRepository.AddProductAsync(product);
            _unitOfWork.SaveChanges();
            return new ProductResponse 
            { Status = true,
              Message = "Product created successfully.",
             Data = _mapper.Map<ProductDto>(product)
            };
        }

        public async Task<ProductResponse> UpdateProductAsync(int productId, ProductDto productDto, int sellerId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.SellerId != sellerId)
                return new ProductResponse { Status = false, Message = "Product not found or not authorized" };

            product.Quantity = productDto.Quantity;
            product.Price = productDto.Price;
            await _productRepository.UpdateProductAsync(product);
            _unitOfWork.SaveChanges();
            return new ProductResponse
            {
                Status = true,
                Message = "Product updated successfully",
                Data = _mapper.Map<ProductDto>(product)
            };
        }

        public async Task<ProductResponse> DeleteProductAsync(int productId)
        {
            await _productRepository.DeleteProductAsync(productId);
            _unitOfWork.SaveChanges();
            return new ProductResponse { Status = true, Message = "Product deleted successfully." };
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new BaseResponse<IEnumerable<ProductDto>>//(false, "Search term cannot be empty.");
                {
                    Status = false,
                    Message = "Search term cannot be empty."
                };
            }

            var products = await _productRepository.SearchProductsAsync(searchTerm);

            if (products == null || !products.Any())
            {
                return new BaseResponse<IEnumerable<ProductDto>>//(false, "No products found.");
                {
                    Status = false,
                    Message = "No products found."
                };
            }

            var productDtos = products.Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl
            }).ToList();

            return new BaseResponse<IEnumerable<ProductDto>>//(true, "Products retrieved successfully.", productDtos);
            {
                Status = true,
                Message = "Products retrieved successfully.",
                Data = (IEnumerable<ProductDto>)productDtos
            };
        }

    }
}

