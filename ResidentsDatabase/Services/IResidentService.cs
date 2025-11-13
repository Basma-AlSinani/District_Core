using ResidentsDatabase.Models;

namespace ResidentsDatabase.Services
{
    public interface IResidentService
    {
        Task AddResidentAsync(Resident resident);
        Task DeleteResidentAsync(string residentId);
        Task<IEnumerable<Resident>> GetAllResidentsAsync();
        Task<Resident?> GetByResidentIdAsync(string residentId);
        Task UpdateResidentAsync(Resident resident);
    }
}