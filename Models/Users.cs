using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public enum UserRole
    {
        Admin=0,
        Investigator=1,
        Officer=2
    }

    public enum ClearanceLevel
    {
        Low=0,
        Medium=1,
        High=2,
        Critical=3
    }
    [Index(nameof(Email), IsUnique = true)]
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

        

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName { get; set; }
        //add unique constraint to email
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public ClearanceLevel ClearanceLevel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CaseParticipants> CaseParticipants { get; set; } = new List<CaseParticipants>();

        public ICollection<CaseComment> CaseComments { get; set; } = new List<CaseComment>();

    }
}
