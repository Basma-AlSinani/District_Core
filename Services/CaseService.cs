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
        private readonly IGenericRepository<CaseReports> _caseReportsRepo;
        private readonly IGenericRepository<Users> _userRepository;

        public CaseService(ICasesRepo caseRepository, IGenericRepository<CrimeReports> crimeReportsRepo, IGenericRepository<CaseReports> caseReportsRepo, IGenericRepository<Users> userRepository)
        {
            _caseRepository = caseRepository;
            _crimeReportsRepo = crimeReportsRepo;
            _caseReportsRepo = caseReportsRepo;
            _userRepository = userRepository;
        }

        // Create a new case
        public async Task<Cases> CreateCaseAsync(CaseCreateDTO dto)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("Description is required.");
            if (string.IsNullOrWhiteSpace(dto.AreaCity))
                throw new ArgumentException("AreaCity is required.");
            if (!Enum.IsDefined(typeof(Status), dto.Status))
                throw new ArgumentException($"Invalid Status value. Allowed values: {string.Join(", ", Enum.GetNames(typeof(Status)))}");
            if (!Enum.IsDefined(typeof(AuthorizationLevel), dto.AuthorizationLevel))
                throw new ArgumentException($"Invalid AuthorizationLevel value. Allowed values: {string.Join(", ", Enum.GetNames(typeof(AuthorizationLevel)))}");

            // Validate CreatedByUserId exists
            var user = await _userRepository.GetByIdAsync(dto.CreatedByUserId);
            if (user == null)
                throw new ArgumentException($"User with ID {dto.CreatedByUserId} does not exist.");

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
                AuthorizationLevel = dto.AuthorizationLevel,
                Status = Status.Pending,
                CreatedByUserId = dto.CreatedByUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _caseRepository.AddAsync(newCase);
            await _caseRepository.SaveChangesAsync();

            // Link crime reports to the new case
            foreach (var reportId in dto.CrimeReportIds)
            {
                var report = await _crimeReportsRepo.GetByIdAsync(reportId);
                if (report == null)
                    throw new ArgumentException($"Crime report with ID {reportId} does not exist.");

                var caseReport = new CaseReports
                {
                    CaseId = newCase.CaseId,
                    CrimeReportId = report.CrimeReportId,
                    PerformedBy = dto.CreatedByUserId
                };
                await _caseReportsRepo.AddAsync(caseReport);

            }
            await _caseReportsRepo.SaveChangesAsync();

            return newCase;
        }

        // Update an existing case
        public async Task<(Cases? updatedCase, string? error)> UpdateCaseAsync(int caseId, UpdateCaseDTO dto)
        {
            // Check if Case exists
            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null)
                return (null, "Case not found");

            // Validate Status value
            if (dto.Status.HasValue && !Enum.IsDefined(typeof(ProgreessStatus), dto.Status.Value))
                return (null, "Invalid Status value");

            // Update fields
            if (!string.IsNullOrEmpty(dto.Description))
                existingCase.Description = dto.Description;

            // Update Status
            if (dto.Status.HasValue)
                existingCase.Status = (Status)(int)dto.Status.Value;

            // Validate AssignedToUserId if provided
            if (dto.AssignedToUserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(dto.AssignedToUserId.Value);
                if (user == null)
                    return (null, $"Assigned user with ID {dto.AssignedToUserId} not found");

                existingCase.CaseReports.Add(new CaseReports
                {
                    CaseId = existingCase.CaseId,
                    CrimeReportId = 0, // Placeholder for actual logic
                    PerformedBy = dto.AssignedToUserId.Value
                });
            }

            // Validate CrimeReportIds exist
            if (dto.AddCrimeReportId != null)
            {
                foreach (var reportId in dto.AddCrimeReportId)
                {
                    var report = await _crimeReportsRepo.GetByIdAsync(reportId);
                    if (report == null)
                        return (null, $"Crime Report with ID {reportId} not found");
                }
            }

            await _caseRepository.UpdateAsync(existingCase);
            await _caseRepository.SaveChangesAsync();

            // Insert related crime reports after validation
            if (dto.AddCrimeReportId != null)
            {
                foreach (var reportId in dto.AddCrimeReportId)
                {
                    var caseReport = new CaseReports
                    {
                        CaseId = existingCase.CaseId,
                        CrimeReportId = reportId
                    };
                    await _caseReportsRepo.AddAsync(caseReport);
                }
                await _caseReportsRepo.SaveChangesAsync();
            }
            return (existingCase, null);
        }

        // Get all cases
        public async Task<IEnumerable<CaseListDTO>> GetCasesAsync()
        {
            var cases = await _caseRepository.GetAllAsync();

            var result = cases.Select(c => new CaseListDTO
            {
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

            var firstReport = caseEntity.CaseReports?
                .FirstOrDefault()?
                .CrimeReports;

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
                ReportedBy = firstReport != null ? $"{firstReport.Users.FirstName} {firstReport.Users.LastName}" : "N/A",
            };
        }

        public async Task<bool> DeleteCaseAsync(int caseId)
        {
            // Check if the case exists
            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null)
                return false;

            // Delete related CaseReports to avoid foreign key constraint issues
            var relatedReports = await _caseReportsRepo
                .GetAllAsync();

            // Delete the case
            var toRemove = relatedReports.Where(r => r.CaseId == caseId).ToList();
            foreach (var item in toRemove)
            {
                await _caseReportsRepo.DeleteAsync(item);
            }

            // delete casas
            await _caseRepository.DeleteAsync(existingCase);

            return true;
        }
    }
}


