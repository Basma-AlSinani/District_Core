using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface IEvidenceRepo : IGenericRepository<Evidence>
    {
        Task<byte[]> GetEvidenceImageAsync(int id);
        Task HardDeletAsync(Evidence evidence);
        Task SoftDeleteAsync(Evidence evidence);
        Task UpdateContentAsync(int id, string? textcontent, string? remarks);
    }
}