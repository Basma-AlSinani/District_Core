
namespace CrimeManagment.Services
{
    public interface IEmailService
    {
        Task SendCaseUpdateAsync(string to, string caseId, string updateDetails);
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendNewCrimeAlertAsync(string to, string crimeDetails);
    }
}