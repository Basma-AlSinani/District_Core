namespace Crime.DTOs
{
    public class ParticipantReadDTO
    {
        // DTO for reading participant details
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }
    }
}
