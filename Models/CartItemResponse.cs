namespace RATAISHOP.Models
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }

    public class CartItemCreateDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemResponse : BaseResponse<CartItemDto>
    {
        public CartItemDto? Data { get; set; }
    }

}
