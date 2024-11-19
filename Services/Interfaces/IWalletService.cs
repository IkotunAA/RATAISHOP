using RATAISHOP.Models;

namespace RATAISHOP.Services.Interfaces
{
    public interface IWalletService
    {
        Task<BaseResponse<WalletDto>> GetWalletBySellerIdAsync(int sellerId);
        Task<BaseResponse<WalletDto>> UpdateWalletAsync(WalletUpdateDto walletDto);
        Task<BaseResponse<WalletDto>> CreateWalletAsync(WalletCreateDto walletDto);
        Task<BaseResponse<WalletDto>> WithdrawFromWalletAsync(int sellerId, decimal amount, string accountNumber, string bankName);
    }

}
