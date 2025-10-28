﻿using Crime.Models;

namespace Crime.Services
{
    public interface IEvidenceService
    {
        Task AddAsync(Evidence evidence);
        Task<IEnumerable<Evidence>> GetAllAsync();
        Task<Evidence> GetByIdAsync(int id);
        Task<byte[]> GetEvidenceImageAsync(int id);
        Task HardDeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task UpdateContentAsync(int id, string? textcontent, string? remarks);
    }
}