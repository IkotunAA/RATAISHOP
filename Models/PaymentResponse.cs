namespace RATAISHOP.Models
{
    public class PaymentDto
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
    public class PaymentResponse : BaseResponse<PaymentDto>
    {
        public PaymentDto? Data { get; set; }
    }
}
