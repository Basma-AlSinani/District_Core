using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICasesRepo
    {
        Task<IEnumerable<Cases>> GetAllAsync();
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        Task<Cases> GetCaseDetailsByIdAsync(int id);
    }
}