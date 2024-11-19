using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.PaymentModel;

namespace RATAISHOP.PaymentServices.Interfaces
{
    public interface IPaystackPaymentService
    {
        Task<PaystackPaymentResponse> InitializePaymentAsync(PaystackPaymentModel paymentRequest);
        Task<OrderResponse> VerifyPaymentAsync(string reference);
        Task<BaseResponse<string>> InitiatePaymentAsync(Order order);
    }
}

