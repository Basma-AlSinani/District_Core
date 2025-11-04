using CrimeManagment.Repositories;
using CrimeManagment.Models;
namespace CrimeManagment.Services
{
    public class CaseReportService : ICaseReportService
    {
        private readonly ICaseReportRepo _caseReportRepo;
        public CaseReportService(ICaseReportRepo caseReportRepo)
        {
            _caseReportRepo = caseReportRepo;
        }

        public async Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId)
        {
            return await _caseReportRepo.GetByCaseAndReportAsync(caseId, crimeReportId);
        }

        public async Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId)
        {
            return await _caseReportRepo.GetByCaseIdAsync(caseId);
        }

        public async Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId)
        {
            return await _caseReportRepo.GetByReportIdAsync(reportId);
        }

        public async Task<IEnumerable<CaseReports>> GetAllAsync()
        {
            return await _caseReportRepo.GetAllAsync();
        }

        public async Task<CaseReports> GetByIdAsync(int id)
        {
            return await _caseReportRepo.GetByIdAsync(id);
        }

        public async Task AddAsync(CaseReports entity)
        {
            await _caseReportRepo.AddAsync(entity);
        }

        public async Task UpdateAsync(CaseReports entity)
        {
            await _caseReportRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _caseReportRepo.GetByIdAsync(id);
            if (entity != null)
            {
                await _caseReportRepo.DeleteAsync(entity);
            }
        }
    }
}
