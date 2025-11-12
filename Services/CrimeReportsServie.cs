using CrimeManagment.Repositories;
using CrimeManagment.Models;
using CrimeManagementApi.DTOs;
using Microsoft.EntityFrameworkCore;
namespace CrimeManagment.Services
{
    public class CrimeReportsService : ICrimeReportsService
    {
        private readonly ICrimeReportsRepository _repo;
        private readonly IEmailService _emailService;
        private readonly IUsersRepo _userRepo;
        public CrimeReportsService(ICrimeReportsRepository repo, IEmailService emailService, IUsersRepo userRepo)
        {
            _repo = repo;
            _emailService = emailService;
            _userRepo = userRepo;
        }

        // Create a new report
        // Create a new report
        public async Task<CrimeReportDto?> CreateReportAsync(CreateCrimeReportDto dto, int? reportedByUserId = null)
        {
            var entity = new CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity ?? string.Empty,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0,
                Email = dto.Email,
                UserId = reportedByUserId,
                CrimeStatus = CrimeStatus.Pending,
                ReportDataTime = DateTime.UtcNow,
            };

            await _repo.AddAsync(entity);

            var allUsers = await _userRepo.GetAllUsersAsync();
            foreach (var user in allUsers)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    string subject = $"New Crime Reported in {entity.AreaCity}";
                    string body = $"A new crime has been reported in {entity.AreaCity}.<br>" +
                                  $"<strong>Title:</strong> {entity.Title}<br>" +
                                  $"<strong>Description:</strong> {entity.Description}<br>" +
                                  $"<strong>Date:</strong> {entity.ReportDataTime}";
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
            }

            return new CrimeReportDto
            {
                Id = entity.CrimeReportId,
                Title = entity.Title,
                Description = entity.Description,
                AreaCity = entity.AreaCity,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Status = entity.CrimeStatus.ToString(),
                ReportDateTime = entity.ReportDataTime
            };
        }



        // Get all reports
        public async Task<IEnumerable<CrimeReportDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();

            return list.Select(entity => new CrimeReportDto
            {
                Id = entity.CrimeReportId,
                Title = entity.Title,
                Description = entity.Description,
                AreaCity = entity.AreaCity,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Status = entity.CrimeStatus.ToString(),
                ReportDateTime = entity.ReportDataTime
            });
        }

        // Get a single report by ID
        public async Task<CrimeReportDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            return new CrimeReportDto
            {
                Id = entity.CrimeReportId,
                Title = entity.Title,
                Description = entity.Description,
                AreaCity = entity.AreaCity,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Status = entity.CrimeStatus.ToString(),
                ReportDateTime = entity.ReportDataTime
            };
        }

        // Get only the status of a report
        public async Task<string> GetStatusAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity?.CrimeStatus.ToString() ?? "Not Found";
        }

        // Search reports by keyword in title or description
        public async Task<IEnumerable<CrimeReportDto>> SearchAsync(string keyword)
        {
            var list = await _repo.SearchAsync(keyword);

            return list.Select(entity => new CrimeReportDto
            {
                Id = entity.CrimeReportId,
                Title = entity.Title,
                Description = entity.Description,
                AreaCity = entity.AreaCity,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Status = entity.CrimeStatus.ToString(),
                ReportDateTime = entity.ReportDataTime
            });
        }

        // Update report status
        public async Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus)
        {
            await _repo.UpdateReportStatusAsync(reportId, newStatus);
        }

        // Delete a report
        public async Task DeleteAsync(int id)
        {
            var report = await _repo.GetByIdAsync(id);
            if (report != null)
            {
                await _repo.DeleteAsync(report);
            }
        }
    }
}


