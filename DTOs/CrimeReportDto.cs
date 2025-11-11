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
        public string? AreaCity { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Range(-90, 90)]
        public decimal? Latitude { get; set; }

        [Range(-180, 180)]
        public decimal? Longitude { get; set; }

    }

    public class CreateCrimeReportsDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "AreaCity is required.")]
        [MaxLength(100, ErrorMessage = "AreaCity cannot exceed 100 characters.")]
        public string? AreaCity { get; set; }
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public decimal? Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
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
    public class CreateCrimeReportByUserDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? AreaCity { get; set; }

        public int ReportedByUserId { get; set; } 
        [Range(-90, 90)]
        public decimal? Latitude { get; set; }

        [Range(-180, 180)]
        public decimal? Longitude { get; set; }
    }

}

