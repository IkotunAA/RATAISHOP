namespace RATAISHOP.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<bool> ProcessPaymentAsync(int paymentDto);
    }
}
