using Crime.Models;

namespace Crime.Services
{
    public interface IUsersService
    {
        Task<Users> CreateAsync(Users user);
        Task DeleteAsync(int id);
        Task<Users> GetByEmailAsync(string email);
        Task<Users> GetByIdAsync(int id);
        Task<Users> GetByUsernameAsync(string username);
        Task UpdateAsync(Users user);
    }
}