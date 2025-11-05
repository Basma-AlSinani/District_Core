using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrimeManagment.Controllers
{
    [Authorize(Roles ="Admin")] // Only Admins can access user management
    [Route("api/[controller]")]
    [ApiController]

    public class UserControllers : ControllerBase
    {
         private readonly IUsersService _userService;

    public UserControllers(IUsersService userService)
    {
        _userService = userService;
    }

        [HttpPost("CreateNewUser")]
        public async Task<IActionResult> CreateUser(UsersCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!IsValidEmail(dto.Email))
                return BadRequest(new { message = "Invalid email format." });

            if (await _userService.GetByEmailAsync(dto.Email) != null)
                return BadRequest(new { message = "Email already exists." });


            var firstName = string.IsNullOrWhiteSpace(dto.FirstName) ? "Unknown" : dto.FirstName;
            var secondName = string.IsNullOrWhiteSpace(dto.SecondName) ? "Unknown" : dto.SecondName;
            var lastName = string.IsNullOrWhiteSpace(dto.LastName) ? "User" : dto.LastName;
            var fullName = $"{firstName} {secondName} {lastName}".Trim();
            if(!Enum.IsDefined(typeof(UserRole), dto.Role))
            {
                return BadRequest(new { message = "Invalid user role. You must choose from: 0.Admin, 1.Investigator, 2.Officer." });
            }
            if(!Enum.IsDefined(typeof(ClearanceLevel), dto.ClearanceLevel))
            {
                return BadRequest(new { message = "Invalid clearance level. You must choose from: 0.Low, 1.Medium, 2.High, 3.Critical." });
            }
            var user = new Users
            {
                FirstName = firstName,
                SecondName = secondName,
                LastName = lastName,
                FullName = fullName,
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = _userService.HashPassword(dto.Password),
                Role = dto.Role,
                ClearanceLevel = dto.ClearanceLevel
            };

            var createdUser = await _userService.CreateAsync(user);

            

            return Ok(createdUser);
        }



        [HttpPut("UpdateByID/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            await _userService.UpdateAsync(id, dto);
            return Ok(new { message = "User updated successfully." });
        }


        [HttpDelete("DeleteByID/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { message = "User deleted successfully." });
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }

        // Get All Users 
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get User By ID
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            return Ok(user);
        }
    }
}
