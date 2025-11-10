using CrimeManagment.Models;
using CrimeManagment.Repositories;

namespace CrimeManagment.Services
{
    public class CaseAssigneesService : ICaseAssigneesService
    {
        private readonly ICaseAssigneesRepo _caseAssigneesRepo;
        private readonly IUsersRepo _userRepo ;
        private readonly ICasesRepo _caseRepo;




        public CaseAssigneesService(ICaseAssigneesRepo caseAssigneesRepo, IUsersRepo userRepo, ICasesRepo caseRepo)
        {
            _caseAssigneesRepo = caseAssigneesRepo;
            _userRepo = userRepo;
            _caseRepo = caseRepo;
        }

        private int RoleRank(UserRole role)
        {
            return role switch
            {
                UserRole.Officer => 1,
                UserRole.Investigator => 2,
                UserRole.Admin => 3,
                _ => 0
            };
        }


        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _caseAssigneesRepo.GetAssigneesByCaseIdAsync(caseId);
        }

        public async Task<bool> AssignUserToCaseAsync(int caseId, int assignerId, int assigneeId, AssigneeRole role)
        {
            var assigner = await _userRepo.GetByIdAsync(assignerId);
            var assignee = await _userRepo.GetByIdAsync(assigneeId);
            var targetCase = await _caseRepo.GetByIdAsync(caseId);

            if (assigner == null || assignee == null || targetCase == null)
                return false;

            
            if (RoleRank(assigner.Role) <= RoleRank(assignee.Role))
                return false;

            
            if ((int)assignee.ClearanceLevel < (int)targetCase.AuthorizationLevel)
                return false;

            
            var newAssignment = new CaseAssignees
            {
                CaseId = caseId,
                CaseAssigneeId = assigneeId,
                Role = role
            };

            await _caseAssigneesRepo.AddAsync(newAssignment);
            await _caseAssigneesRepo.SaveChangesAsync();

            return true;
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
