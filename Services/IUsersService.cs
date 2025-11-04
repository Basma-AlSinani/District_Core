using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface IUsersService
    {
        Task<Users> CreateAsync(Users user);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateUserDto dto);
        string HashPassword(string password);
        Task<bool> ExistsByUsername(string username);
        Task<Users> AuthenticateAsync(string email, string password);
        Task<Users?> GetByEmailAsync(string email);

    }
}