namespace RATAISHOP.Entities
{
    public class PaymentRequest
    {
        public int BuyerId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Paystack or Wallet
    }
}
