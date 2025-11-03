using CrimeManagment.Repositories;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public class EvidenceAuditLogsService : IEvidenceAuditLogsService
    {
        private readonly IEvidenceAuditLogsRepo _evidenceAuditLogsRepo;
        public EvidenceAuditLogsService(IEvidenceAuditLogsRepo evidenceAuditLogsRepo)
        {
            _evidenceAuditLogsRepo = evidenceAuditLogsRepo;
        }

        public async Task<IEnumerable<EvidenceAuditLogs>> GetAllAsync()
        {
            return await _evidenceAuditLogsRepo.GetAllAsync();
        }

        public async Task<EvidenceAuditLogs> GetByIdAsync(int id)
        {
            var log = await _evidenceAuditLogsRepo.GetByIdAsync(id);
            if (log == null)
                throw new Exception("Audit log not found");
            return log;
        }

        public async Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId)
        {
            return await _evidenceAuditLogsRepo.GetLogsByEvidenceIdAsync(evidenceId);
        }

        public async Task AddAsync(EvidenceAuditLogs log)
        {
            await _evidenceAuditLogsRepo.AddAsync(log);
        }

    }
}
