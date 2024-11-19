using RATAISHOP.Enum;

namespace RATAISHOP.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public User Buyer { get; set; } = default!;
        public User Seller { get; set; } = default!;
        public string BuyerEmail { get; set; }
        public int SellerId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; } = default!;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentReference { get; set; } = default!;
        public string ShippingMethod { get; set; } = default!;

    }
}
