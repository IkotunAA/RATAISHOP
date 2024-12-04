using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Enum;
using RATAISHOP.Models;
using RATAISHOP.Services.Interfaces;
using System.Threading.Tasks;

namespace RATAISHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.UserName) ||
                string.IsNullOrEmpty(registerRequest.Email) ||
                string.IsNullOrEmpty(registerRequest.Password) ||
                string.IsNullOrEmpty(registerRequest.Address) ||
                string.IsNullOrEmpty(registerRequest.PhoneNumber))
            {
                return BadRequest(new BaseResponse<string>
                {
                    Status = false,
                    Message = "All fields are required."
                });
            }

            // Automatically set WalletBalance to 0 for sellers
            var userDto = new UserDto
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Address = registerRequest.Address,
                PhoneNumber = registerRequest.PhoneNumber,
                Role = registerRequest.Role,
                WalletBalance = registerRequest.Role == UserRole.Seller ? 0 : (decimal?)null
            };

            var response = await _userService.RegisterUser(userDto);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Identifier) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new BaseResponse<string>
                {
                    Status = false,
                    Message = "Username or email and password are required."
                });
            }

            var response = await _userService.LoginUserByIdentifier(loginRequest.Identifier, loginRequest.Password);

            if (!response.Status)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}
