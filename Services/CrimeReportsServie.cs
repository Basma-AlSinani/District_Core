using CrimeManagment.Repositories;
using CrimeManagment.Models;
using CrimeManagementApi.DTOs;
using Microsoft.EntityFrameworkCore;
namespace CrimeManagment.Services
{
    public class CrimeReportsServies : ICrimeReportsServie
    {
        private readonly ICrimeReportsRepository _repo;
        public CrimeReportsServies(ICrimeReportsRepository repo)
        {
            _repo = repo;
        }

        // Add a new CrimeReports entity
        public async Task AddAsync(CrimeReports report)
        {
            await _repo.AddAsync(report);
        }

        // Create a new report
        public async Task<CrimeReportDto?> CreateReportAsync(CreateCrimeReportDto dto)
        {
            var entity = new CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity ?? string.Empty,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0,
                CrimeStatus = CrimeStatus.Pending,
                ReportDataTime = DateTime.UtcNow,
                UserId = null
            };

            await _repo.AddAsync(entity);

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


