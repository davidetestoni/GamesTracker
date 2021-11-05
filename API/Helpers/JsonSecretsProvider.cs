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

        public string SendinblueApiKey { get; set; }

        public string SmtpServer { get; private set; }
        public int SmtpPort { get; private set; }
        public string SmtpUsername { get; private set; }
        public string SmtpPassword { get; private set; }

        public JsonSecretsProvider(string fileName)
        {
            var json = File.ReadAllText(fileName);
            var obj = JObject.Parse(json);
            TwitchAppId = obj["twitch_app_id"].ToString();
            TwitchAppSecret = obj["twitch_app_secret"].ToString();
            JwtIssuerKey = obj["jwt_issuer_key"].ToString();
            SendinblueApiKey = obj["sendinblue_api_key"].ToString();
            SmtpServer = obj["smtp_server"].ToString();
            SmtpPort = obj["smtp_port"].Value<int>();
            SmtpUsername = obj["smtp_username"].ToString();
            SmtpPassword = obj["smtp_password"].ToString();
        }
    }
}
