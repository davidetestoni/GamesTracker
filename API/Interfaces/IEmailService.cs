using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Provides a way to send emails to users.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// The address that will be set in the email's 'from' field.
        /// </summary>
        string SenderAddress { get; set; }

        /// <summary>
        /// Sends an email message to the <paramref name="address"/> with the specified
        /// <paramref name="subject"/> and <paramref name="body"/>.
        /// </summary>
        Task SendAsync(string address, string subject, string body, bool isHtml = false);
    }
}
