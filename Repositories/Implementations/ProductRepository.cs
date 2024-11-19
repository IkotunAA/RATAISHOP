using Microsoft.EntityFrameworkCore;
using RATAISHOP.Context;
using RATAISHOP.Entities;
using RATAISHOP.Repositories.Interfaces;

namespace RATAISHOP.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly RataiDbContext _context;

        public ProductRepository(RataiDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            {
                return await _context.Products
                    .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                    .ToListAsync();
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}
