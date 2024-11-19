namespace RATAISHOP.Models
{
    public class TransactionResponse
    {
        public TransactionDto? Data { get; set; }
    }
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}
