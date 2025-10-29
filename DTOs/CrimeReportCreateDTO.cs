namespace Crime.DTOs
{
    public class CrimeReportCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? UserId { get; set; }
    }
}
