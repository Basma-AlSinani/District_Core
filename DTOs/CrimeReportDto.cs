using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CrimeManagementApi.DTOs
{

    public class CrimeReportDto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? AreaCity { get; set; }
        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending";
        public DateTime ReportDateTime { get; set; } = DateTime.UtcNow;
        [Range(-90, 90)]
        public decimal? Latitude { get; set; }
        [Range(-180, 180)]
        public decimal? Longitude { get; set; }
    }

    public class CreateCrimeReportDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(100)]
        [AllowNull]
        public string? AreaCity { get; set; }
        [AllowNull]
        public int? ReportedByUserId { get; set; }
        [Range(-90, 90)]
        public decimal? Latitude { get; set; }
        [Range(-180, 180)]
        public decimal? Longitude { get; set; }
    }

    public class CreateCrimeReportsDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(100)]
        [AllowNull]
        public string? AreaCity { get; set; }
        [Range(-90, 90)]
        public decimal? Latitude { get; set; }
        [Range(-180, 180)]
        public decimal? Longitude { get; set; }
    }

    public class UpdateCrimeReportDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? AreaCity { get; set; }
        [MaxLength(50)]
        public string? Status { get; set; }
        [Range(-90, 90)]
        public decimal? Latitude { get; set; }
        [Range(-180, 180)]
        public decimal? Longitude { get; set; }
    }
}

