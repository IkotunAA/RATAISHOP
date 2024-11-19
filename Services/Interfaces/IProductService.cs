using RATAISHOP.Models;

namespace RATAISHOP.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> GetProductByIdAsync(int productId);
        Task<ProductResponse> GetAllProductAsync();
        Task<ProductResponse> CreateProductAsync(ProductDto productDto, int sellerId);
        Task<ProductResponse> UpdateProductAsync(int productId, ProductDto productDto, int sellerId);
        Task<ProductResponse> DeleteProductAsync(int producId);
        Task<BaseResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm);


    }

}
