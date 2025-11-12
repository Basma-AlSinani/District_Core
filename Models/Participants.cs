using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public class Participants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantsId { get; set; }

        [Required,MaxLength(200)]
        public string FullName { get; set; }

        [Required,MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [Required]
        public int CaseId { get; set; }
    }
}
