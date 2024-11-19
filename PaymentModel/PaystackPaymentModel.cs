using RATAISHOP.Models;

namespace RATAISHOP.PaymentModel
{
    public class PaystackPaymentModel
    {
        public string Email { get; set; } = default!;
        public decimal Amount { get; set; } 
        public string CallbackUrl { get; set; } = default!;
        public string Reference { get; set; }
    }
    public class PaystackPaymentResponse : BaseResponse<string>
    {
        public string? AuthorizationUrl { get; set; }
        public string? AccessCode { get; set; }
        public string? Reference { get; set; }
        public string? Status { get; set; } 
    }

}
