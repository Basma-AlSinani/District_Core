using CrimeManagment.Repositories;
using CrimeManagment.Models;
using CrimeManagment.DTOs;
using static CrimeManagment.DTOs.EvidenceDTOS;


namespace CrimeManagment.Services
{
    public class EvidenceService : IEvidenceService
    {
        private readonly IEvidenceRepo _evidenceRepo;
        private readonly IUsersRepo _userRepo;
        public EvidenceService(IEvidenceRepo evidenceRepo,IUsersRepo usersRepo)
        {
            _evidenceRepo = evidenceRepo;
            _userRepo = usersRepo;
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
        public async Task<Evidence> CreateTextEvidenceAsync(CreateTextEvidenceRequest request)
        {
            var user= await _userRepo.GetByIdAsync(request.CreatedByUserId);
            if (user==null)
                throw new Exception("User not found");

            var evidence = new Evidence
            {
                CaseId = request.CaseId,
                Type = EvidenceType.Text,
                TextContent = request.TextContent,
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow,
                AddedByUser = user
            };

            await _evidenceRepo.AddAsync(evidence);
            return evidence;
        }

        public async Task<Evidence> CreateImageEvidenceAsync(EvidenceDTOS.CreateImageEvidenceRequest request)
        {
            var user = await _userRepo.GetByIdAsync(request.CreatedByUserId);
            if (user == null)
                throw new Exception("User not found");
            if (request.File == null)
                throw new ArgumentException("Image file is required.");

            if (!request.File.ContentType.StartsWith("image/"))
                throw new ArgumentException("Invalid image format.");

            var uploadDirectory = Path.Combine("wwwroot", "evidences");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var evidence = new Evidence
            {
                CaseId = request.CaseId,
                Type = EvidenceType.Image,
                FileUrl = filePath,
                MimeType = request.File.ContentType,
                SizeBytes = request.File.Length,
                Remarks = request.Remarks,
                CreatedAt = DateTime.UtcNow,
                AddedByUser = user
            };

            await _evidenceRepo.AddAsync(evidence);
            return evidence;
        }

    }
}


