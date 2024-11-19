using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RATAISHOP.Models;
using RATAISHOP.Services.Interfaces;

namespace RATAISHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            if (!response.Status)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var response = await _userService.RegisterUser(userDto);
            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest(new BaseResponse<string> { Status = false, Message = "ID mismatch" });

            var response = await _userService.UpdateUserAsync(userDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            return Ok(response);
        }
    }

}
