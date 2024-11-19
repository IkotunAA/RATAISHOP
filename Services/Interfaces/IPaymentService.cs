using RATAISHOP.Entities;
using RATAISHOP.Models;
using System.Threading.Tasks;

namespace RATAISHOP.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseResponse<bool>> TransferToBankAccountAsync(string accountNumber, string bankCode, decimal amount, string narration, string bankName);
        Task<BaseResponse<string>> ProcessBankTransferPayment(Order order);
        Task<BaseResponse<string>> ProcessPaystackPayment(Order order);
        Task<BaseResponse<string>> ConfirmBankTransfer(int orderId, string transactionReference);
    }

}
