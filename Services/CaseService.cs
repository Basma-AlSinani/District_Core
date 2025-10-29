using Crime.DTOs;
using Crime.Models;
using Crime.Repositories;

namespace Crime.Services
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
    }
}

