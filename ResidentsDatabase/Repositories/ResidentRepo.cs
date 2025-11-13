using Microsoft.EntityFrameworkCore;
using ResidentsDatabase.Models;

namespace ResidentsDatabase.Repositories
{
    public class ResidentRepo : IResidentRepo
    {
        private readonly ResidentsDbContext _context;
        public ResidentRepo(ResidentsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resident>> GetAllResidentsAsync()
        {
            return await _context.Residents.ToListAsync();
        }

        public async Task<Resident?> GetByResidentIdAsync(string residentId)
        {
            return await _context.Residents
                .FirstOrDefaultAsync(r => r.NationalId == residentId);
        }

        public async Task AddResidentAsync(Resident resident)
        {
            await _context.Residents.AddAsync(resident);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResidentAsync(Resident resident)
        {
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResidentAsync(string residentId)
        {
            var resident = await GetByResidentIdAsync(residentId);
            if (resident != null)
            {
                _context.Residents.Remove(resident);
                await _context.SaveChangesAsync();
            }
        }
    }
}