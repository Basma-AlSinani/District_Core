using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum Role
    {
        Admin,
        Investigator,
        Officer
    }

    public enum ClearanceLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string SecondName { get; set; }

        [MaxLength(50)]
        public string ThirdName { get; set; } 

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public ClearanceLevel ClearanceLevel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
