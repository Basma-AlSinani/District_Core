using ResidentsDatabase.Repositories;

namespace ResidentsDatabase.Services
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepo _residentRepo;
        public ResidentService(IResidentRepo residentRepo)
        {
            _residentRepo = residentRepo;
        }
        public async Task AddResidentAsync(Models.Resident resident)
        {
            await _residentRepo.AddResidentAsync(resident);
        }
        public async Task<IEnumerable<Models.Resident>> GetAllResidentsAsync()
        {
            return await _residentRepo.GetAllResidentsAsync();
        }

        public async Task<Models.Resident?> GetByResidentIdAsync(string residentId)
        {
            return await _residentRepo.GetByResidentIdAsync(residentId);
        }

        public async Task UpdateResidentAsync(Models.Resident resident)
        {
            await _residentRepo.UpdateResidentAsync(resident);
        }

        public async Task DeleteResidentAsync(string residentId)
        {
            await _residentRepo.DeleteResidentAsync(residentId);
        }
    }
}
