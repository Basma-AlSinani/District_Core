using Crime.Models;

namespace Crime.Services
{
    public interface ICaseService
    {
        Task<Cases> CreateAsync(Cases newCase);
        Task DeleteAsync(int id);
        Task<IEnumerable<Cases>> GetAllAsync();
        Task<Cases> GetByIdAsync(int id);
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        Task UpdateAsync(Cases updatedCase);
    }
}