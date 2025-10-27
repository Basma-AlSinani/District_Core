
using Crime.Models;
namespace Crime.Repositories
{
    public class EvidenceRepo : GenericRepository<Evidence>, IEvidenceRepo
    {
        private readonly CrimeDbContext _context;
        public EvidenceRepo(CrimeDbContext context) : base(context)
        {
        }

        //soft delete evidence  
        public async Task SoftDeleteAsync(Evidence evidence)
        {
            evidence.IsSoftDeleted = true;
            evidence.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(evidence);
        }

        //call the function to hard delete evidence
        public async Task HardDeletAsync(Evidence evidence)
        {
            await DeleteAsync(evidence);
        }

        //reutern the evidence image as byte array
        public async Task<byte[]> GetEvidenceImageAsync(int id)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            if (evidence.Type != EvidenceType.Image)
                throw new Exception("Evidence is not an image");

            if (string.IsNullOrEmpty(evidence.FileUrl))
                throw new Exception("Image file URL is null or empty");

            return await File.ReadAllBytesAsync(evidence.FileUrl);
        }

        //Update text evidence content (the type cannot change)
        public async Task UpdateContentAsync(int id, string? textcontent, string? remarks)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            if (evidence.Type != EvidenceType.Text)
                throw new Exception("Evidence is not of type Text");

            evidence.TextContent = textcontent;
            evidence.Remarks = remarks;
            evidence.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(evidence);
        }
    }
}
