using Crime.Models;

namespace Crime.Repositories
{
    public interface IEvidenceRepo 
    {
        Task<byte[]> GetEvidenceImageAsync(int id);
        Task HardDeletAsync(Evidence evidence);
        Task SoftDeleteAsync(Evidence evidence);
        Task UpdateContentAsync(int id, string? textcontent, string? remarks);
    }
}