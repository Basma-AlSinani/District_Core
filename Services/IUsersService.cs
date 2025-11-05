using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface IUsersService
    {
        Task<Users?> AuthenticateAsync(string email, string password);
        Task<Users> CreateAsync(Users user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByUsername(string username);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<Users?> GetByEmailAsync(string email);
        Task<UserDTO?> GetUserByIdAsync(int id);
        string HashPassword(string password);
        Task UpdateAsync(int id, UpdateUserDto dto);
    }
}