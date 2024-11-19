using Microsoft.EntityFrameworkCore;
using RATAISHOP.Context;
using RATAISHOP.Entities;
using RATAISHOP.Repositories.Interfaces;

namespace RATAISHOP.Repositories.Implementations
{
    public class WalletRepository : IWalletRepository
    {
        private readonly RataiDbContext _context;

        public WalletRepository(RataiDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetWalletBySellerIdAsync(int sellerId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.SellerId == sellerId);
        }

        public async Task UpdateWalletAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> CreateWalletAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }
    }
}
