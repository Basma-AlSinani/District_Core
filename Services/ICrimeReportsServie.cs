using CrimeManagementApi.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICrimeReportsServie
    {
        Task AddAsync(CrimeReports report);
        Task<CrimeReportDto?> CreateReportAsync(CreateCrimeReportDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CrimeReportDto>> GetAllAsync();
        Task<CrimeReportDto?> GetByIdAsync(int id);
        Task<string> GetStatusAsync(int id);
        Task<IEnumerable<CrimeReportDto>> SearchAsync(string keyword);
        Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus);
    }
}