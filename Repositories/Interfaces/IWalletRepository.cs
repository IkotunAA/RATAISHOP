using RATAISHOP.Entities;

namespace RATAISHOP.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetWalletBySellerIdAsync(int sellerId);
        Task UpdateWalletAsync(Wallet wallet);
        Task<Wallet> CreateWalletAsync(Wallet wallet);
    }
}
