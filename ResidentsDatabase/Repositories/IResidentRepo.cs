using ResidentsDatabase.Models;

namespace ResidentsDatabase.Repositories
{
    public interface IResidentRepo
    {
        Task AddResidentAsync(Resident resident);
        Task DeleteResidentAsync(string residentId);
        Task<IEnumerable<Resident>> GetAllResidentsAsync();
        Task<Resident?> GetByResidentIdAsync(string residentId);
        Task UpdateResidentAsync(Resident resident);
    }
}