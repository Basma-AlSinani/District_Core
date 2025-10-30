using Crime.DTOs;
using Crime.Models;
using Crime.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    [Route("api/Users")]
    [ApiController]

    public class UserControllers : ControllerBase
    {
         private readonly IUsersService _userService;

    public UserControllers(IUsersService userService)
    {
        _userService = userService;
    }

        [HttpPost("Create New User")]
        public async Task<IActionResult> CreateUser([FromBody] UsersCreateDTO dto)
        {
            // Map UsersCreateDTO to Users model
            var user = new Users
            {
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                ThirdName = dto.ThirdName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = dto.Password,
                Role = dto.Role,
                ClearanceLevel = dto.ClearanceLevel
            };

            var CreateUser = await _userService.CreateAsync(user);
            return Ok(CreateUser);
        }

        [HttpPut("Update By ID/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            await _userService.UpdateAsync(id, dto);
            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("Delete By ID/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }
    }
}
