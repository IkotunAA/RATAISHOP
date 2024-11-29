using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName) ||
                string.IsNullOrEmpty(userDto.Password) ||
                string.IsNullOrEmpty(userDto.Email))
            {
                return BadRequest(new BaseResponse<string>
                {
                    Status = false,
                    Message = "Username, password, and email are required."
                });
            }

            var response = await _userService.RegisterUser(userDto);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest(new BaseResponse<string>
                {
                    Status = false,
                    Message = "Username and password are required."
                });
            }

            var response = await _userService.LoginUser(userDto);

            if (!response.Status)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}
