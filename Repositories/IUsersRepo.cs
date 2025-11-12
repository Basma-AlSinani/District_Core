using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface IUsersRepo : IGenericRepository<Users>
    {
        Task<Users> GetByEmailAsync(string email);
        Task<Users> GetByUsernameAsync(string username);
        Task<IEnumerable<Users>> GetAllUsersAsync();
    }
}