using System.ComponentModel.DataAnnotations;

namespace CrimeManagement.DTOs
{
    public class AuthDtos
    {

        public class LoginDtos
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
            public string Password { get; set; }
        }

        public class LoginResponseDto
        {
            public string Email { get; set; }
            public string Role { get; set; }
            public string Token { get; set; }
        }
    }
}
