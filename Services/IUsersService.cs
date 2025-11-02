using Crime.DTOs;
using Crime.Models;

namespace Crime.Services
{
    public interface IUsersService
    {
        Task<Users> CreateAsync(Users user);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateUserDto dto);
        string HashPassword(string password);

    }
}