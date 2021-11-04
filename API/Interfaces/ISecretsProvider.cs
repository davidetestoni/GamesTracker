namespace API.Interfaces
{
    public interface ISecretsProvider
    {
        string TwitchAppId { get; }
        string TwitchAppSecret { get; }
        string JwtIssuerKey { get; }
    }
}
