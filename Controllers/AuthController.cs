using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Protocols.WSIdentity;
using RATAISHOP.Authentication;
using RATAISHOP.Models;
using RATAISHOP.Services.Interfaces;
using TokenService = RATAISHOP.Authentication.TokenService;

namespace RATAISHOP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto registerDto)
        {
            var response = await _userService.RegisterUser(registerDto);
            if (!response.Status == false)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto loginDto)
        {
            var response = await _userService.LoginUser(loginDto);
            if (!response.Status == false)
            {
                return Unauthorized(response.Message);
            }
            return Ok(response.Data);
        }
    }

}