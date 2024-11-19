namespace RATAISHOP.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
