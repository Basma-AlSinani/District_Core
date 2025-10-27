namespace District_Core.Models
{
    public class CrimeReports
    {
        public int CrimeReportId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public DateTime ReportDataTime { get; set; }
        public string Status { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
