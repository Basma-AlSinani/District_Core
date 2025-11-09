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

                if (existingCase.CaseReports == null)
                    existingCase.CaseReports = await _caseRepository.GetCaseReportsByCaseIdAsync(existingCase.CaseId);

                if (existingCase.CaseReports != null)
                {
                    foreach (var caseReport in existingCase.CaseReports)
                    {
                        var report = await _crimeReportsRepo.GetByIdAsync(caseReport.CrimeReportId);
                        if (report != null)
                            report.CrimeStatus = dto.CrimeStatus.Value;
                    }
                }
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

            if (dto.AddCrimeReportId != null)
            {
                if (existingCase.CaseReports == null)
                    existingCase.CaseReports = await _caseRepository.GetCaseReportsByCaseIdAsync(existingCase.CaseId);

                var existingReportIds = existingCase.CaseReports.Select(cr => cr.CrimeReportId).ToHashSet();

                foreach (var reportId in dto.AddCrimeReportId)
                {
                    var report = await _crimeReportsRepo.GetByIdAsync(reportId);
                    if (report == null)
                        return (null, $"Crime Report with ID {reportId} not found");

                    if (!existingReportIds.Contains(reportId))
                    {
                        existingCase.CaseReports.Add(new CaseReports
                        {
                            CaseId = existingCase.CaseId,
                            CrimeReportId = reportId,
                            PerformedBy = existingCase.CreatedByUserId
                        });
                        existingReportIds.Add(reportId);
                    }
                }
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

            int suspectsCount = caseEntity.CaseParticipants?
                .Count(p => p.Role == Role.Suspect) ?? 0;

            int victimsCount = caseEntity.CaseParticipants?
                .Count(p => p.Role == Role.Victim) ?? 0;

            int witnessesCount = caseEntity.CaseParticipants?
                .Count(p => p.Role == Role.Witness) ?? 0;


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


                NumberOfSuspects = suspectsCount,
                NumberOfVictims = victimsCount,
                NumberOfWitnesses = witnessesCount
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


