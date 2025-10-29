using Crime.Repositories;
using Crime.Models;

namespace Crime.Services
{
    public class CaseAssigneesService : ICaseAssigneesService
    {
        private readonly ICaseAssigneesRepo _caseAssigneesRepo;
        public CaseAssigneesService(ICaseAssigneesRepo caseAssigneesRepo)
        {
            _caseAssigneesRepo = caseAssigneesRepo;
        }

        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _caseAssigneesRepo.GetAssigneesByCaseIdAsync(caseId);
        }

        public async Task<bool> AssignUserToCaseAsync(int caseId, int userId, AssigneeRole role)
        {
            return await _caseAssigneesRepo.AssignUserToCaseAsync(caseId, userId, role);
        }

        public async Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus)
        {
            return await _caseAssigneesRepo.UpdateAssigneeStatusAsync(caseAssigneeId, newStatus);
        }

        public async Task<IEnumerable<CaseAssignees>> GetAllAsync()
        {
            return await _caseAssigneesRepo.GetAllAsync();
        }

        public async Task<CaseAssignees> GetByIdAsync(int id)
        {
            return await _caseAssigneesRepo.GetByIdAsync(id);
        }

        public async Task AddAsync(CaseAssignees entity)
        {
            await _caseAssigneesRepo.AddAsync(entity);
        }

        public async Task UpdateAsync(CaseAssignees entity)
        {
            await _caseAssigneesRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _caseAssigneesRepo.GetByIdAsync(id);
            if (entity != null)
            {
                await _caseAssigneesRepo.DeleteAsync(entity);
            }
        }
    }
}
