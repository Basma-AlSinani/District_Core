using CrimeManagment.Models;
using CrimeManagment.Repositories;

namespace CrimeManagment.Services
{
    public class CaseAssigneesService : ICaseAssigneesService
    {
        private readonly ICaseAssigneesRepo _caseAssigneesRepo;

        public CaseAssigneesService(ICaseAssigneesRepo caseAssigneesRepo)
        {
            _caseAssigneesRepo = caseAssigneesRepo;
        }

        private int RoleRank(UserRole role)
        {
            return role switch
            {
                UserRole.Admin => 4,
                UserRole.Investigator => 3,
                UserRole.Officer => 2,
                _ => 0
            };
        }

        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _caseAssigneesRepo.GetAssigneesByCaseIdAsync(caseId);
        }

        public async Task<bool> AssignUserToCaseAsync(int caseId, int assignerId, int assigneeId, AssigneeRole role)
        {
            var assigner = await _caseAssigneesRepo.GetUserByIdAsync(assignerId);
            var assignee = await _caseAssigneesRepo.GetUserByIdAsync(assigneeId);
            if (assigner == null || assignee == null) return false;

            if (RoleRank(assigner.Role) <= RoleRank(assignee.Role)) return false;

            if (role == AssigneeRole.Officer && (int)assignee.ClearanceLevel < await _caseAssigneesRepo.GetCaseAuthorizationLevel(caseId))
                return false;

            return await _caseAssigneesRepo.AssignUserToCaseAsync(caseId, assigneeId, role);
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
            if (entity != null) await _caseAssigneesRepo.DeleteAsync(entity);
        }
    }
}
