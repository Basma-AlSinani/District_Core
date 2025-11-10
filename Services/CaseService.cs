using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICasesRepo _caseRepository;
        private readonly IGenericRepository<CrimeReports> _crimeReportsRepo;
        private readonly IGenericRepository<Users> _userRepository;

        public CaseService(ICasesRepo caseRepository, IGenericRepository<CrimeReports> crimeReportsRepo, IGenericRepository<Users> userRepository)
        {
            _caseRepository = caseRepository;
            _crimeReportsRepo = crimeReportsRepo;
            _userRepository = userRepository;
        }

        // Create a new case
        public async Task<Cases> CreateCaseAsync(CaseCreateDTO dto, int currentUserId, UserRole currentUserRole)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("Description is required.");
            if (string.IsNullOrWhiteSpace(dto.AreaCity))
                throw new ArgumentException("AreaCity is required.");

            AuthorizationLevel level = currentUserRole switch
            {
                UserRole.Admin => AuthorizationLevel.Critical,
                UserRole.Investigator => AuthorizationLevel.High,
                UserRole.Officer => AuthorizationLevel.Medium,
                _ => AuthorizationLevel.Low
            };

            // Generate a unique case number
            string uniqueCaseNumber = $"CASE-{DateTime.UtcNow:yyyyMMddHHmmss}";
            // Ensure the case number is unique
            var existingCase = await _caseRepository.GetCaseByNumberAsync(uniqueCaseNumber);
            if (existingCase != null)
                throw new Exception("Case number already exists.");

            // Create new case
            var newCase = new Cases
            {
                CaseNumber = uniqueCaseNumber,
                Name = dto.Name,
                Description = dto.Description,
                AreaCity = dto.AreaCity,
                CaseType = dto.CaseType,
                AuthorizationLevel = level,
                Status = Status.Pending,
                CreatedByUserId = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _caseRepository.AddAsync(newCase);
            await _caseRepository.SaveChangesAsync();

            return newCase;
        }

        // Update an existing case
        public async Task<(Cases? updatedCase, string? error)> UpdateCaseAsync(int caseId, UpdateCaseDTO dto)
        {
            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null)
                return (null, "Case not found");

            if (dto.Status.HasValue && !Enum.IsDefined(typeof(ProgreessStatus), dto.Status.Value))
                return (null, "Invalid Status value");

            if (!string.IsNullOrEmpty(dto.Description))
                existingCase.Description = dto.Description;

            if (dto.Status.HasValue)
            {
                existingCase.Status = dto.Status.Value switch
                {
                    ProgreessStatus.Pending => Status.Pending,
                    ProgreessStatus.InProgress => Status.Ongoing,
                    ProgreessStatus.Completed => Status.Closed,
                    ProgreessStatus.Closed => Status.Closed,
                    _ => existingCase.Status
                };
            }
            if (dto.AssignedToUserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(dto.AssignedToUserId.Value);
                if (user == null)
                    return (null, $"Assigned user with ID {dto.AssignedToUserId} not found");

                if (existingCase.CaseAssignees == null)
                    existingCase.CaseAssignees = new List<CaseAssignees>();

                existingCase.CaseAssignees.Add(new CaseAssignees
                {
                    CaseId = existingCase.CaseId,
                    AssignedToUserId = dto.AssignedToUserId.Value,
                    AssignedByUserId = existingCase.CreatedByUserId,
                    Role = AssigneeRole.Investigator,
                    Status = ProgreessStatus.Pending,
                    AssignedAt = DateTime.UtcNow
                });
            }

            await _caseRepository.UpdateAsync(existingCase);
            await _caseRepository.SaveChangesAsync();

            return (existingCase, null);
        }



        // Get all cases
        public async Task<IEnumerable<CaseListDTO>> GetCasesAsync()
        {
            var cases = await _caseRepository.GetAllAsync();

            var result = cases.Select(c => new CaseListDTO
            {
                CaseId = c.CaseId.ToString(),
                CaseNumber = c.CaseNumber,
                Name = c.Name,
                Description = c.Description,
                AreaCity = c.AreaCity,
                CaseType = c.CaseType,
                AuthorizationLevel = c.AuthorizationLevel,
                CreatedBy = $"{c.CreatedByUser.FirstName} {c.CreatedByUser.LastName}",
                CreatedAt = c.CreatedAt
            });

            return result;
        }

        public async Task<CaseDetailsDTO> GetCaseDetailsAsync(int id)
        {
            var caseEntity = await _caseRepository.GetCaseDetailsByIdAsync(id);
            if (caseEntity == null)
                return null;

            return new CaseDetailsDTO
            {
                CaseNumber = caseEntity.CaseNumber,
                Name = caseEntity.Name,
                Description = caseEntity.Description,
                AreaCity = caseEntity.AreaCity,
                CaseType = caseEntity.CaseType,
                CaseLevel = "Level 1",
                AuthorizationLevel = caseEntity.AuthorizationLevel.ToString(),
                CreatedBy = $"{caseEntity.CreatedByUser.FirstName} {caseEntity.CreatedByUser.LastName}",
                CreatedAt = caseEntity.CreatedAt,
                ReportedBy = "N/A",
            };
        }

        public async Task<bool> DeleteCaseAsync(int caseId)
        {
            // Check if the case exists
            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null)
                return false;

            // delete casas
            await _caseRepository.DeleteAsync(existingCase);

            return true;
        }
    }
}


