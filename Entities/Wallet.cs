namespace RATAISHOP.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; } = default!;
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
