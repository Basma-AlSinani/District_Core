using CrimeManagment.Models;

namespace CrimeManagment.DTOs
{
    public class UpdateUserDto
    {
        public string? Password { get; set; }
        public UserRole? Role { get; set; }
        public ClearanceLevel? ClearanceLevel { get; set; }
        public string? Email { get; set; }
    }
}
