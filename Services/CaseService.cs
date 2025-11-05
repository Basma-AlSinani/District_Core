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

        public CaseService(ICasesRepo caseRepository, IGenericRepository<CrimeReports> crimeReportsRepo, IGenericRepository<CaseReports> caseReportsRepo)
        {
            _caseRepository = caseRepository;
            _crimeReportsRepo = crimeReportsRepo;
            _caseReportsRepo = caseReportsRepo;
        }

        // Create a new case
        public async Task<Cases> CreateCaseAsync(CaseCreateDTO dto)
        {
            // Generate a unique case number
            string uniqueCaseNumber = $"CASE-{DateTime.UtcNow:yyyyMMddHHmmss}";

            // Ensure the case number is unique
            var existingCase = await _caseRepository.GetCaseByNumberAsync(uniqueCaseNumber);
            if (existingCase != null)
                throw new Exception("Case number already exists.");

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
                if (report != null)
                {
                    var caseReport = new CaseReports
                    {
                        CaseId = newCase.CaseId,
                        CrimeReportId = report.CrimeReportId,
                        PerformedBy = dto.CreatedByUserId
                    };
                    await _caseReportsRepo.AddAsync(caseReport);
                }
            }
            await _caseReportsRepo.SaveChangesAsync();

            return newCase;
        }

        // Update an existing case
        public async Task<Cases> UpdateCaseAsync(int caseId, UpdateCaseDTO dto)
        {
            var existingCase = await _caseRepository.GetByIdAsync(caseId);
            if (existingCase == null) throw new Exception("Case not found");

            if (!string.IsNullOrEmpty(dto.Description))
                existingCase.Description = dto.Description;

            if (dto.Status.HasValue)
                existingCase.Status = (Status)(int)dto.Status.Value;

            await _caseRepository.UpdateAsync(existingCase);
            await _caseRepository.SaveChangesAsync();

            if (dto.AddCrimeReportId != null)
            {
                foreach (var reportId in dto.AddCrimeReportId)
                {
                    var report = await _crimeReportsRepo.GetByIdAsync(reportId);
                    if (report != null)
                    {
                        var caseReport = new CaseReports
                        {
                            CaseId = existingCase.CaseId,
                            CrimeReportId = report.CrimeReportId
                        };
                        await _caseReportsRepo.AddAsync(caseReport);
                    }
                }
                await _caseReportsRepo.SaveChangesAsync();
            }

            return existingCase;
        }

        // Submit crime report
        public async Task<CrimeReports> SubmitCrimeReportAsync(CrimeReportCreateDTO dto)
        {
            var report = new CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ReportDataTime = DateTime.UtcNow,
                CrimeStatus = CrimeStatus.reported,
                UserId = dto.UserId ?? 0
            };

            await _crimeReportsRepo.AddAsync(report);
            await _crimeReportsRepo.SaveChangesAsync();

            return report;
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
                throw new Exception("Case not found");

            int assigneesCount = 0;
            int evidencesCount = 0;
            int suspectsCount = 0;
            int victimsCount = 0;
            int witnessesCount = 0;

            var firstReport = caseEntity.CaseReports?.FirstOrDefault()?.CrimeReports;

            return new CaseDetailsDTO
            {
                CaseNumber = caseEntity.CaseNumber,
                Name = caseEntity.Name,
                Description = caseEntity.Description,
                AreaCity = caseEntity.AreaCity,
                CaseType = caseEntity.CaseType,
                CaseLevel = "Level 1", // Placeholder for case level
                AuthorizationLevel = caseEntity.AuthorizationLevel.ToString(),
                CreatedBy = $"{caseEntity.CreatedByUser.FirstName} {caseEntity.CreatedByUser.LastName}",
                CreatedAt = caseEntity.CreatedAt,
                ReportedBy = firstReport != null ? $"{firstReport.Users.FirstName} {firstReport.Users.LastName}" : "N/A",
                NumberOfAssignees = assigneesCount,
                NumberOfEvidences = evidencesCount,
                NumberOfSuspects = suspectsCount,
                NumberOfVictims = victimsCount,
                NumberOfWitnesses = witnessesCount
            };
        }

        // Get assignees by case ID
        public async Task<IEnumerable<object>> GetAssigneesByCaseIdAsync(int caseId)
        {
            var assignees = await _caseRepository.GetAssigneesByCaseIdAsync(caseId);
            return assignees.Select(a => new
            {
                a.UserId,
                FullName = $"{a.FirstName} {a.LastName}"
            });
        }

        // Get evidence by case ID
        public async Task<IEnumerable<object>> GetEvidenceByCaseIdAsync(int caseId)
        {
            var evidences = await _caseRepository.GetEvidenceByCaseIdAsync(caseId);
            return evidences.Select(e => new
            {
                e.EvidenceId,
                e.Type,
                e.TextContent,
                e.FileUrl,
                e.MimeType,
                e.SizeBytes,
                e.Remarks,
                e.CreatedAt
            });
        }

        // Get participants by role
        public async Task<IEnumerable<object>> GetParticipantsByRoleAsync(int caseId, Role role)
        {
            var participants = await _caseRepository.GetParticipantsByRoleAsync(caseId, role);
            return participants.Select(p => new
            {
                p.ParticipantId,
                FullName = p.Participant.FullName,
                Role = p.Role.ToString(),
                p.AssignedAt
            });
        }
    }
}


