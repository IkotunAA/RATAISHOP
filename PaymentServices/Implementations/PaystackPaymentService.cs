using Newtonsoft.Json;
using PayStack.Net;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.PaymentModel;
using RATAISHOP.PaymentServices.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RATAISHOP.PaymentServices.Implementations
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private readonly PayStackApi _paystackApi;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;


        public PaystackPaymentService(PayStackApi paystackApi, IConfiguration configuration, HttpClient httpClient)
        {
            _paystackApi = paystackApi;
            _configuration = configuration;
            _httpClient = httpClient;
            
        }

        public async Task<PaystackPaymentResponse> InitializePaymentAsync(PaystackPaymentModel paymentRequest)
        {
            var transactionRequest = new TransactionInitializeRequest
            {
                Email = paymentRequest.Email,
                AmountInKobo = (int)(paymentRequest.Amount * 100), // Convert Naira to Kobo
                CallbackUrl = paymentRequest.CallbackUrl
            };

            var response = _paystackApi.Transactions.Initialize(transactionRequest);

            if (response.Status)
            {
                return new PaystackPaymentResponse
                {
                    Status = "Successful",
                    Message = "Payment initialization successful",
                    AuthorizationUrl = response.Data.AuthorizationUrl,
                    AccessCode = response.Data.AccessCode,
                    Reference = response.Data.Reference
                };
            }

            return new PaystackPaymentResponse
            {
                Status = "Failed",
                Message = response.Message
            };
        }

        public async Task<BaseResponse<string>> InitiatePaymentAsync(Order order)
        {
            var paystackApiKey = _configuration["Paystack:ApiKey"];
            var paystackApiUrl = _configuration["Paystack:ApiUrl"];

            var request = new PaystackPaymentModel
            {
                Amount = (int)(order.TotalAmount * 100), // Convert to kobo
                Email = order.Buyer.Email,
                Reference = Guid.NewGuid().ToString(),
                CallbackUrl = "https://yourdomain.com/paystack/callback"
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paystackApiKey);

            var response = await _httpClient.PostAsync($"{paystackApiUrl}/transaction/initialize", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<PaystackPaymentResponse>(responseContent);

                return new BaseResponse<string> { Status = true, Message = "Paystack payment initialized.", Data = responseObject.AuthorizationUrl};
            }

            return new BaseResponse<string> { Status = false, Message = "Paystack payment failed.", Data = null };
        }

        public async Task<OrderResponse> VerifyPaymentAsync(string reference)
        {
            var response = _paystackApi.Transactions.Verify(reference);

            if (response.Status && response.Data.Status == "success")
            {
                return new OrderResponse
                {
                    Status = true,
                    Message = "Payment verified successfully"
                };
            }

            return new OrderResponse
            {
                Status = false,
                Message = "Payment verification failed",
                //Data = respo
            };
        }
    }
}
