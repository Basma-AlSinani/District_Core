using CrimeManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace CrimeManagment.DTOs
{
    public class UsersCreateDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second name is required.")]
        public string SecondName { get; set; }

       // public string? ThirdName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public ClearanceLevel ClearanceLevel { get; set; }
    }
}
