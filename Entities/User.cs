using RATAISHOP.Enum;

namespace RATAISHOP.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public decimal WalletBalance { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
