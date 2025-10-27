using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class UsersRepo : GenericRepository<Users>, IUsersRepo
    {
        private readonly CrimeDbContext _context;
        public UsersRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Users> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
