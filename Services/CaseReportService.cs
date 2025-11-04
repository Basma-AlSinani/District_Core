using CrimeManagment.Repositories;
using CrimeManagment.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            return await _caseReportRepo.GetAllWithRelationsAsync();
        }

        public async Task<CaseReports> GetByIdAsync(int id)
        {
            return await _caseReportRepo.GetDetailsAsync(id);
        }

        public async Task AddAsync(CaseReports entity)
        {
            await _caseReportRepo.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(CaseReports entity)
        {
            var existing = await _caseReportRepo.GetByIdAsync(entity.CaseReportId);
            if (existing == null)
                return false;

            await _caseReportRepo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _caseReportRepo.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _caseReportRepo.DeleteAsync(entity);
            return true;
        }
    }
}


