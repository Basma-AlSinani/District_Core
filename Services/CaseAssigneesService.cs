using CrimeManagment.Models;
using CrimeManagment.Repositories;

using static CrimeManagment.DTOs.CaseAssigneesDTOs;

namespace CrimeManagment.Services
{
    public class CaseAssigneesService : ICaseAssigneesService
    {
        private readonly ICaseAssigneesRepo _caseAssigneesRepo;
        private readonly IUsersRepo _userRepo;
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


        public async Task<IEnumerable<CaseAssigneeDTOs>> GetAssigneesByCaseIdAsync(int caseId)
        {
            var assignees = await _caseAssigneesRepo.GetAssigneesByCaseIdAsync(caseId);

            return assignees.Select(a => new CaseAssigneeDTOs
            {
                CaseAssigneeId = a.CaseAssigneeId,
                CaseId = a.CaseId,
                Role = a.Role.ToString(),
                Status = a.Status.ToString(),
                AssignedAt = a.AssignedAt,
                AssignedToName = a.AssignedTo?.FullName,
                AssignedToRole = a.AssignedTo?.Role.ToString(),
                AssignedByName = a.AssignedBy?.FullName,
                CaseNumber = a.Cases?.CaseNumber,
                CaseName = a.Cases?.Name
            });
        }


        public async Task<(bool Success, string Message)> AssignUserToCaseAsync(int caseId, int assignerId, int assigneeId, AssigneeRole role)
        {
            var assigner = await _userRepo.GetByIdAsync(assignerId);
            var assignee = await _userRepo.GetByIdAsync(assigneeId);
            var targetCase = await _caseRepo.GetByIdAsync(caseId);

            if (assigner == null)
                return (false, $"Assigner with ID {assignerId} not found.");
            if (assignee == null)
                return (false, $"Assignee with ID {assigneeId} not found.");
            if (targetCase == null)
                return (false, $"Case with ID {caseId} not found.");

            if (RoleRank(assigner.Role) <= RoleRank(assignee.Role))
                return (false, $"Cannot assign. {assigner.Role} cannot assign a {assignee.Role}.");

            if ((int)assignee.ClearanceLevel < (int)targetCase.AuthorizationLevel)
                return (false, $"Assignee's clearance level is too low for this case.");

            // Optional: prevent duplicates
            var existing = (await _caseAssigneesRepo.GetAssigneesByCaseIdAsync(caseId))
                .FirstOrDefault(a => a.AssignedToUserId == assigneeId);
            if (existing != null)
                return (false, "This user is already assigned to this case.");

            var newAssignment = new CaseAssignees
            {
                CaseId = caseId,
                AssignedByUserId = assignerId,
                AssignedToUserId = assigneeId,
                Role = role,
                Status = ProgreessStatus.Pending,
                AssignedAt = DateTime.UtcNow
            };

            await _caseAssigneesRepo.AddAsync(newAssignment);
            await _caseAssigneesRepo.SaveChangesAsync();

            return (true, "Assignment successful.");
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