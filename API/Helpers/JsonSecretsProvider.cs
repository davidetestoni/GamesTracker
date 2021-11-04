using API.Interfaces;
using Newtonsoft.Json.Linq;
using System.IO;

namespace API.Helpers
{
    public class JsonSecretsProvider : ISecretsProvider
    {
        public string TwitchAppId { get; private set; }
        public string TwitchAppSecret { get; private set; }
        public string JwtIssuerKey { get; private set; }

        public JsonSecretsProvider(string fileName)
        {
            var json = File.ReadAllText(fileName);
            var obj = JObject.Parse(json);
            TwitchAppId = obj["twitch_app_id"].ToString();
            TwitchAppSecret = obj["twitch_app_secret"].ToString();
            JwtIssuerKey = obj["jwt_issuer_key"].ToString();
        }
    }
}
