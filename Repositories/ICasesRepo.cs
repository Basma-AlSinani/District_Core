using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICasesRepo : IGenericRepository<Cases>
    {
        Task<IEnumerable<Cases>> GetAllAsync();
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        Task<Cases> GetCaseDetailsByIdAsync(int id);
    }
}