using API.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISecretsProvider _secretsProvider;

        public string SenderAddress { get; set; } = "noreply@videogamestracker.herokuapp.com";

        public EmailService(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
        }

        public async Task SendAsync(string address, string subject, string body, bool isHtml = false)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SenderAddress));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = subject;
            email.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
            {
                Text = body
            };

            using var ms = new MemoryStream();
            var protocolLogger = new ProtocolLogger(ms);
            using var smtp = new SmtpClient(protocolLogger);

            try
            {
                await smtp.ConnectAsync(_secretsProvider.SmtpServer, _secretsProvider.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_secretsProvider.SmtpUsername, _secretsProvider.SmtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch
            {
                var log = Encoding.UTF8.GetString(ms.ToArray());
                throw;
            }
        }
    }
}
