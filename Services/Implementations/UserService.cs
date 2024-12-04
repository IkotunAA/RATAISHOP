using AutoMapper;
using RATAISHOP.Authentication;
using RATAISHOP.Entities;
using RATAISHOP.Models;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;
using System.Threading.Tasks;

namespace RATAISHOP.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, TokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new UserResponse { Status = false, Message = "User not found." };
            }

            return new UserResponse { Status = true, Data = _mapper.Map<UserDto>(user) };
        }

        public async Task<UserResponse> RegisterUser(UserDto userDto)
        {
            var newUser = await _userRepository.GetByIdAsync(userDto.Id);
            if (newUser?.UserName != null)
            {
                return new UserResponse
                {
                    Status = false,
                    Message = "Username already exists"
                };
            }
            if (newUser?.Email != null)
            {
                return new UserResponse//(false, "User already exists");
                {
                    Status = false,
                    Message = "User already exists"
                };
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password, salt);
            //var user = _mapper.Map<User>(userDto);
            var user = new User()
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = hashedPassword,
                Address = userDto.Address,
                Role = userDto.Role,
                PhoneNumber = userDto.PhoneNumber,
                WalletBalance = 0,
            };
            await _userRepository.AddAsync(user);
            _unitOfWork.SaveChanges();
            return new UserResponse { Status = true, Message = "User created successfully." };
        }

        public async Task<UserResponse> UpdateUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.UpdateAsync(user);
            _unitOfWork.SaveChanges();
            return new UserResponse { Status = true, Message = "User updated successfully." };
        }

        public async Task<BaseResponse<string>> DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
            return new BaseResponse<string> { Status = true, Message = "User deleted successfully." };
        }

        public async Task<LoginResponseModel> LoginUserByIdentifier(string identifier, string password)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(identifier);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new LoginResponseModel
                {
                    Status = false,
                    Message = "Invalid username/email or password."
                };
            }

            var token = _tokenService.GenerateToken(user.UserName);

            return new LoginResponseModel
            {
                Status = true,
                Message = "Login successful.",
                Token = token,
                //Data = _mapper.Map<UserDto>(user)
            };
        }
    }

}
