using CrimeManagment.Repositories;
using CrimeManagment.Models;
using CrimeManagment.DTOs;


namespace CrimeManagment.Services
{
    public class EvidenceService : IEvidenceService
    {
        private readonly IEvidenceRepo _evidenceRepo;
        public EvidenceService(IEvidenceRepo evidenceRepo)
        {
            _evidenceRepo = evidenceRepo;
        }

        public async Task<IEnumerable<Evidence>> GetAllAsync()
        {
            return await _evidenceRepo.GetAllAsync();
        }

        public async Task<Evidence> GetByIdAsync(int id)
        {
            var evidence = await _evidenceRepo.GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");
            return evidence;
        }

        public async Task AddAsync(Evidence evidence)
        {
            await _evidenceRepo.AddAsync(evidence);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            await _evidenceRepo.SoftDeleteAsync(evidence);
        }

        public async Task HardDeleteAsync(int id)
        {
            var evidence = await GetByIdAsync(id);
            if (evidence == null)
                throw new Exception("Evidence not found");

            await _evidenceRepo.HardDeletAsync(evidence);
        }

        public async Task<byte[]> GetEvidenceImageAsync(int id)
        {
            return await _evidenceRepo.GetEvidenceImageAsync(id);
        }

        public async Task UpdateContentAsync(int id, string? textcontent, string? remarks)
        {
            await _evidenceRepo.UpdateContentAsync(id, textcontent, remarks);
        }

        public async Task<Evidence> RecordEvidenceAsync(EvidenceDTOS.EvidenceCreateRequest request)
        {
            var evidence = new Evidence
            {
                CaseId = request.CaseId,
                Type = request.Type,
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow,
                AddedByUser = request.CreatedByUser,
            };

            if (request.Type == EvidenceType.Text)
            {
                evidence.TextContent = request.TextContent;
            }
            else
                if (request.Type == EvidenceType.Image && request.File != null)
            {
                var uploadDictory = Path.Combine("wwroot", "evidences");
                if (!Directory.Exists(uploadDictory))
                {
                    Directory.CreateDirectory(uploadDictory);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
                var filePath = Path.Combine(uploadDictory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                evidence.FileUrl = filePath;
                evidence.MimeType = request.File.ContentType;
                evidence.SizeBytes = request.File.Length;
            }
            await _evidenceRepo.AddAsync(evidence);
            return evidence;
        }
    }
}


