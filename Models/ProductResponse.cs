using Microsoft.Identity.Client;
using RATAISHOP.Entities;

namespace RATAISHOP.Models
{
    public class ProductResponse : BaseResponse<ProductDto>
    {
        public ProductDto Productdto { get; set; }
        public Product Product { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile ImageUrl { get; set; } = null!;
        public IList<IFormFile> Images { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile PrimaryImageUrl { get; set; } = null!;
        public IList<IFormFile> Images { get; set; }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile PrimaryImageUrl { get; set; } = null!;
        public IList<IFormFile> Images { get; set; }
    }
}
