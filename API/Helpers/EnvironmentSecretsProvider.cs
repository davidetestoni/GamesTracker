using API.Interfaces;
using System;

namespace API.Helpers
{
    public class EnvironmentSecretsProvider : ISecretsProvider
    {
        public string TwitchAppId { get; private set; }
        public string TwitchAppSecret { get; private set; }

        public string JwtIssuerKey { get; private set; }

        public string SendinblueApiKey { get; private set; }

        public string SmtpServer { get; private set; }
        public int SmtpPort { get; private set; }
        public string SmtpUsername { get; private set; }
        public string SmtpPassword { get; private set; }

        public EnvironmentSecretsProvider()
        {
            TwitchAppId = Environment.GetEnvironmentVariable("TWITCH_APP_ID");
            TwitchAppSecret = Environment.GetEnvironmentVariable("TWITCH_APP_SECRET");
            JwtIssuerKey = Environment.GetEnvironmentVariable("JWT_ISSUER_KEY");
            SendinblueApiKey = Environment.GetEnvironmentVariable("SENDINBLUE_API_KEY");
            SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
            SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));
            SmtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            SmtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        }
    }
}
