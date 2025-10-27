using Crime.Models;
using Crime.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Crime.Services
{
    public class CaseService
    {
        private readonly ICasesRepo _caseRepository;

        public CaseService(ICasesRepo caseRepository)
        {
            _caseRepository = caseRepository;
        }

        // Get all cases
        public async Task<IEnumerable<Cases>> GetAllAsync()
        {
            return await _caseRepository.GetAllAsync();
        }

        // Get case by ID
        public async Task<Cases> GetByIdAsync(int id)
        {
            return await _caseRepository.GetByIdAsync(id);
        }

        // Create a new case
        public async Task<Cases> CreateAsync(Cases newCase)
        {
            // Generate a unique case number
            newCase.CaseNumber = $"CASE-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";

            await _caseRepository.AddAsync(newCase);
            await _caseRepository.SaveChangesAsync();
            return newCase;
        }

        // Update an existing case
        public async Task UpdateAsync(Cases updatedCase)
        {
            await _caseRepository.UpdateAsync(updatedCase);
            await _caseRepository.SaveChangesAsync();
        }

        // Delete a case by ID
        public async Task DeleteAsync(int id)
        {
            {
                var caseToDelete = await _caseRepository.GetByIdAsync(id);
                if (caseToDelete != null)
                {
                    await _caseRepository.DeleteAsync(caseToDelete);
                    await _caseRepository.SaveChangesAsync();
                }
            }
        }

        // Get case by case number
        public async Task<Cases> GetCaseByNumberAsync(string caseNumber)
        {
            return await _caseRepository.GetCaseByNumberAsync(caseNumber);
        }
    }
}

