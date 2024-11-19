namespace RATAISHOP.Models
{
    public class WalletResponse
    {
        public WalletDto? Data { get; set; }
    }
    public class WalletDto
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public decimal Balance { get; set; }
    }
    public class WalletCreateDto
    {
        public int SellerId { get; set; }
        public decimal Balance { get; set; }
    }

    public class WalletUpdateDto
    {
        public decimal Balance { get; set; }
        public int SellerId { get; set; }

    }
}
