using Crime.Models;

namespace Crime.Repositories
{
    public interface IUsersRepo : IGenericRepository<Users>
    {
        Task<Users> GetByEmailAsync(string email);
        Task<Users> GetByUsernameAsync(string username);
    }
}