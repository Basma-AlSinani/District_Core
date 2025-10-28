using Crime.Repositories;
using Crime.Models;

namespace Crime.Services
{
    public class EvidenceService : IEvidenceService
    {
        private readonly IEvidenceRepo _evidenceRepo;
        public EvidenceService(IEvidenceRepo evidenceRepo)
        {
            _evidenceRepo = evidenceRepo;
        }

        public async Task<IEnumerable<Evidence>> GetAllAsync()
        {
            return await _evidenceRepo.GetAllAsync();
        }

        public async Task<Evidence> GetByIdAsync(int id)
        {
            var evidence = await _evidenceRepo.GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");
            return evidence;
        }

        public async Task AddAsync(Evidence evidence)
        {
            await _evidenceRepo.AddAsync(evidence);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            await _evidenceRepo.SoftDeleteAsync(evidence);
        }

        public async Task HardDeleteAsync(int id)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            await _evidenceRepo.HardDeletAsync(evidence);
        }

        public async Task<byte[]> GetEvidenceImageAsync(int id)
        {
            return await _evidenceRepo.GetEvidenceImageAsync(id);
        }

        public async Task UpdateContentAsync(int id, string? textcontent, string? remarks)
        {
            await _evidenceRepo.UpdateContentAsync(id, textcontent, remarks);
        }
    }
}
