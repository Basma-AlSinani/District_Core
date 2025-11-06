namespace CrimeManagment.DTOs
{
    public class EmailDTOs
    {
        public class EmailRequest
        {
            public string To { get; set; } = string.Empty;
            public string Subject { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
        }
        public class CaseUpdateRequest
        {
            public string To { get; set; }
            public string CaseId { get; set; }
            public string UpdateDetails { get; set; }
        }

        public class CrimeAlertRequest
        {
            public string To { get; set; }
            public string CrimeDetails { get; set; }
            public string Location { get; set; }
            public DateTime DateTime { get; set; }
        }
    }
}
