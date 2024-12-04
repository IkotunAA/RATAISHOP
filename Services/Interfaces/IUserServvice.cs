using RATAISHOP.Models;

namespace RATAISHOP.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> RegisterUser(UserDto userDto);
        Task<LoginResponseModel> LoginUserByIdentifier(string identifier, string password);
        Task<UserResponse> UpdateUserAsync(UserDto userDto);
        Task<BaseResponse<string>> DeleteUserAsync(int id);
    }

}
