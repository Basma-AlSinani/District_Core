using Crime.Models;
using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
{
    // DTO for creating a new user
    public class UsersCreateDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } 

        [Required]
        public Role Role { get; set; }

        [Required]
        public ClearanceLevel ClearanceLevel { get; set; }
    }
}
