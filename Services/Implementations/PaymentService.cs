using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RATAISHOP.Entities;
using RATAISHOP.Enum;
using RATAISHOP.Models;
using RATAISHOP.PaymentServices.Implementations;
using RATAISHOP.Repositories.Implementations;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace RATAISHOP.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly PaystackPaymentService _paystackOptions;
        private readonly IOrderRepository _orderRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBankTransferVerificationService _bankTransferVerificationService;
        private readonly HttpClient _httpClient;

        public PaymentService(IOptions<PaystackPaymentService> paystackOptions, IOrderRepository orderRepository, IWalletRepository walletRepository, IProductRepository productRepository, IBankTransferVerificationService bankTransferVerificationService, HttpClient httpClient)
        {
            _paystackOptions = paystackOptions.Value;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
            _orderRepository = orderRepository;
            _walletRepository = walletRepository;
            _productRepository = productRepository;
            _bankTransferVerificationService = bankTransferVerificationService;
        }

        public async Task<BaseResponse<string>> ProcessBankTransferPayment(Order order)
        {
            order.PaymentStatus = PaymentStatus.PendingVerification; // Set status to PendingVerification
            await _orderRepository.UpdateOrderAsync(order);
            var bankDetails = "Bank Name: Example Bank, Account Number: 1234567890, Account Name: Example Business";
            //return new BaseResponse<string>(true, "Bank transfer initiated. Please transfer the amount to the following account: " + bankDetails, null);

            return new BaseResponse<string>//(true, "Bank transfer initiated. Please confirm payment.", null);
            {
                Status = true,
                Message = "Bank transfer initiated. Please confirm payment." + bankDetails
            };
        }
        public async Task<BaseResponse<string>> ConfirmBankTransfer(int orderId, string transactionReference)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null || order.PaymentStatus != PaymentStatus.PendingVerification)
            {
                return new BaseResponse<string>{ Status = false, Message = "Invalid order or order not in pending verification state.", Data = null };
            }

            // Step 3: Verify the bank transfer using a transaction reference
            var isVerified = await _bankTransferVerificationService.VerifyTransaction(transactionReference);

            if (isVerified)
            {
                order.PaymentStatus = PaymentStatus.Paid;
                await _orderRepository.UpdateOrderAsync(order);

                // Step 5: Distribute payments to the respective sellers
                foreach (var orderItem in order.OrderItems)
                {
                    var sellerId = orderItem.Product.SellerId;
                    var wallet = await _walletRepository.GetWalletBySellerIdAsync(sellerId);

                    wallet.Balance += orderItem.TotalPrice;
                    await _walletRepository.UpdateWalletAsync(wallet);
                }

                return new BaseResponse<string>{ Status = true, Message = "Bank transfer verified successfully.", Data = null };
            }
            else
            {
                return new BaseResponse<string>{ Status = false, Message = "Bank transfer verification failed. Please try again.", Data = null };
            }
        }

        public async Task<BaseResponse<string>> ProcessPaystackPayment(Order order)
    {
        var paymentResponse = await _paystackOptions.InitiatePaymentAsync(order);

        if (paymentResponse.Status == true)
        {
            order.PaymentStatus = PaymentStatus.Paid; // Set status to Paid
            await _orderRepository.UpdateOrderAsync(order);

                // Update the seller's wallet
                foreach (var orderItem in order.OrderItems)
                {
                    var sellerId = orderItem.Product.SellerId;
                    var wallet = await _walletRepository.GetWalletBySellerIdAsync(sellerId);

                    wallet.Balance += orderItem.TotalPrice; // Add product's total price to the seller's wallet
                    await _walletRepository.UpdateWalletAsync(wallet);
                }
                return new BaseResponse<string>//(true, "Payment successful via Paystack.", paymentResponse.Data);
                {
                    Status = true,
                    Message = "Payment successful via Paystack.",
                    Data = paymentResponse.Data
                };
        }

        order.PaymentStatus = PaymentStatus.Failed; // Set status to Failed if payment didn't succeed
        await _orderRepository.UpdateOrderAsync(order);

            return new BaseResponse<string>
            {
                Status = false,
                Message = "Paystack payment failed.",
                Data = null
            };
    }

    public async Task<BaseResponse<bool>> TransferToBankAccountAsync(string accountNumber, string bankCode, decimal amount, string narration, string bankName)
    {
        var url = "https://api.paystack.co/transfer";

        var payload = new
        {
            source = "balance",
            amount = (int)(amount * 100), // Paystack requires amount in kobo
            currency = "NGN",
            recipient = new
            {
                type = "nuban",
                name = "Seller Name", // Replace with actual seller name
                account_number = accountNumber,
                bank_code = bankCode,
                Narration = narration,
                BankName = bankName
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            return new BaseResponse<bool>//(false, "Transfer failed.");
            {
                Status = false,
                Message = "Transfer failed."
            };
        }

        return new BaseResponse<bool>
        {
            Status = true,
            Message = "Transfer successful.",
            Data = true
        };
    }
}
}
