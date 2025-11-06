using System.ComponentModel.DataAnnotations;

namespace CrimeManagment.Models
{
    public class EmailSettings
    {
        // SMTP server address(smpt.example.com)
        [Required, MaxLength(150)]
        public string SmtpServer { get; set; } = string.Empty;

        // SMTP port number(usually 25, 465, or 587)
        [Range(1, 65535)]
        public int SmtpPort { get; set; }

        // Sender's display name its required and max length 100
        [Required, MaxLength(100)]
        public string SenderName { get; set; }

        // Sender's email address its required and must be valid email format
        [Required, EmailAddress]
        public string SenderEmail { get; set; }

        [Required]
        public string Password { get; set; }

        // Use SSL for secure email transmission
        public bool EnableSsl { get; set; }

        //enable or disable email notifications
        public bool EnableEmailNotifications { get; set; }=true;
    }
}
