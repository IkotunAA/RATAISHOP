namespace RATAISHOP.Services.Interfaces
{
    public interface IBankTransferVerificationService
    {
        Task<bool> VerifyTransaction(string transactionReference);
    }
}
