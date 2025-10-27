using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class ParticipantsRepo : GenericRepository<Participants>, IParticipantsRepo
    {
        private readonly CrimeDbContext _context;
        public ParticipantsRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Participants> GetByPhoneAsync(string phone)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Phone == phone);
        }
    }
}
