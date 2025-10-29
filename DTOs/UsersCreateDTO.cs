using Crime.Models;
using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
{
    // DTO for creating a new user
    public class UsersCreateDTO
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string? ThirdName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; } 

        public UserRole Role { get; set; }

        public ClearanceLevel ClearanceLevel { get; set; }
    }
}
