using ResidentsDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace ResidentsDatabase.DTOs
{
    public class ListResidentDTO
    {
        [Required(ErrorMessage = "National ID is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "National ID must be exactly 8 characters")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(200, ErrorMessage = "Full name cannot exceed 200 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 digits")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }
    public class AddResidentDTO
    {
        [Required, StringLength(8, MinimumLength = 8)]
        public string NationalId { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MiddleName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ThirdName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public OmanGovernorate City { get; set; }

        [Required, MaxLength(15)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

