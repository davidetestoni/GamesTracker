using API.Exceptions;
using API.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Services
{
    public class SendinblueEmailService : IEmailService
    {
        private readonly ISecretsProvider _secretsProvider;
        public string SenderAddress { get; set; } = "noreply@videogamestracker.herokuapp.com";

        public SendinblueEmailService(ISecretsProvider secretsProvider)
        {
            _secretsProvider = secretsProvider;
        }

        public async Task SendAsync(string address, string subject, string body, bool isHtml = false)
        {
            using var request = new HttpRequestMessage();
            using var httpClient = new HttpClient();
            request.RequestUri = new Uri($"{"https"}://api.sendinblue.com/v3/smtp/email");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("api-key", _secretsProvider.SendinblueApiKey);
            request.Method = HttpMethod.Post;

            var email = new Email
            {
                Sender = new Address(SenderAddress),
                To = new Address[]
                {
                    new Address(address)
                },
                Subject = subject,
                HtmlContent = body
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var content = JsonSerializer.Serialize(email, options);

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            try
            {
                using var response = await httpClient.SendAsync(request);

                if ((int)response.StatusCode / 100 != 2)
                {
                    throw new EmailException($"The sendinblue API did not reply with a 2XX status code");
                }
            }
            catch (Exception ex)
            {
                throw new EmailException("Something went wrong when contacting the sendinblue API", ex);
            }
        }

        private class Email
        {
            public Address Sender { get; set; }
            public Address[] To { get; set; }
            public string Subject { get; set; }
            public string HtmlContent { get; set; }
        }

        private class Address
        {
            public Address(string address)
            {
                Name = address;
                Email = address;
            }

            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
