namespace RATAISHOP.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = default!;
        public int SellerId { get; set; }
        public User Seller { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
