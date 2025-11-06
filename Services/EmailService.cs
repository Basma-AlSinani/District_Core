using CrimeManagment.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net.Mail;
using System.Runtime;

namespace CrimeManagment.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));

            if (!_settings.EnableEmailNotifications)
                throw new InvalidOperationException("Email notifications are disabled in the settings.");
        }
        // Send email asynchronously
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            if (!_settings.EnableEmailNotifications)
                return;

            using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new System.Net.NetworkCredential(_settings.SenderEmail, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
            Console.WriteLine($"Email sent to {to} with subject '{subject}'");


        }

        //case status update notification
        public async Task SendCaseUpdateAsync(string to, string caseId, string updateDetails)
        {
            string subject = $"Update on Case #{caseId}";
            string body = $"Case Update<p>{updateDetails}</p>";
            await SendEmailAsync(to, subject, body);

        }
        //new crime alert notification
        public async Task SendNewCrimeAlertAsync(string to, string crimeDetails)
        {
            string subject = "New Crime Alert";
            string body = $"A new crime has been reported:<p>{crimeDetails}</p>";
            await SendEmailAsync(to, subject, body);
        }

    }
}
