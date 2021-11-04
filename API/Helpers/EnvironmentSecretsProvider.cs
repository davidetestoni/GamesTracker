using API.Interfaces;
using System;

namespace API.Helpers
{
    public class EnvironmentSecretsProvider : ISecretsProvider
    {
        public string TwitchAppId { get; private set; }
        public string TwitchAppSecret { get; private set; }
        public string JwtIssuerKey { get; private set; }

        public EnvironmentSecretsProvider()
        {
            TwitchAppId = Environment.GetEnvironmentVariable("TWITCH_APP_ID");
            TwitchAppSecret = Environment.GetEnvironmentVariable("TWITCH_APP_SECRET");
            JwtIssuerKey = Environment.GetEnvironmentVariable("JWT_ISSUER_KEY");
        }
    }
}
