using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IEmailService
    {
        string SenderAddress { get; set; }

        Task SendAsync(string address, string subject, string body, bool isHtml = false);
    }
}
