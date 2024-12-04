using RATAISHOP.Entities;

namespace RATAISHOP.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User> GetUserByUsernameOrEmailAsync(string identifier);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }

}
