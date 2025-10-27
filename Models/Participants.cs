using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Participants
    {
        [Key]
        public int ParticipantsId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }
    }
}
