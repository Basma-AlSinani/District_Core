using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class CasesRepo : GenericRepository<Cases>, ICasesRepo
    {
        private readonly CrimeDbContext _context;
        public CasesRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cases> GetCaseByNumberAsync(string caseNumber)
        {
            return await _context.Cases
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.CaseNumber == caseNumber);
        }
    }
}

