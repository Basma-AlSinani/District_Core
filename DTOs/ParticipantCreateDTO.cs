using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
{
    // DTO for creating a new participant
    public class ParticipantCreateDTO
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Notes { get; set; }
    }
}
