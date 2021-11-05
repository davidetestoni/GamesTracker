namespace API.Interfaces
{
    public interface ISecretsProvider
    {
        string TwitchAppId { get; }
        string TwitchAppSecret { get; }
        
        string JwtIssuerKey { get; }

        string SendinblueApiKey { get; }

        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }
    }
}
