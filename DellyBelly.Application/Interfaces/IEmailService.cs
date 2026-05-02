using System.Threading.Tasks;

namespace DellyBelly.Application.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a single email to a user.
        /// </summary>
        Task<bool> SendEmailAsync(string toEmail, string subject, string body, string purpose, bool isHtml = true);

        /// <summary>
        /// Sends a circular/broadcast email to multiple recipients at once.
        /// </summary>
        Task<int> SendBroadcastAsync(IEnumerable<string> toEmails, string subject, string body, string purpose);
        
        // Quota Tracking
        Task LogEmailAsync(string recipient, string subject, string purpose, bool success);
        Task<int> GetRemainingQuotaAsync();
    }
}
