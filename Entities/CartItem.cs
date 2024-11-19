namespace RATAISHOP.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public User Buyer { get; set; } = default!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}
