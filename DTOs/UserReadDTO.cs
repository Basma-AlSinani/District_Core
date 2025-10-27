using Crime.Models;

namespace Crime.DTOs
{
    public class UserReadDTO
    {
        // DTO for reading user details
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public ClearanceLevel ClearanceLevel { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
