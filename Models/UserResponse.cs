using RATAISHOP.Enum;

namespace RATAISHOP.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public decimal WalletBalance { get; set; }
        public UserRole Role { get; set; }
    }

    public class UserResponse : BaseResponse <UserDto>
    {
        public UserDto? Data { get; set; }
        public string Token { get; set; }
    }
    public class LoginResponseModel : BaseResponse<UserDto>
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public decimal WalletBalance { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }

}
