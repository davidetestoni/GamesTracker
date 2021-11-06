using API.Helpers;
using API.Interfaces;
using API.Services;
using System.Threading.Tasks;
using Xunit;

namespace GamesTracker.Tests
{
    public class EmailServiceFixture
    {
        public IEmailService EmailService { get; set; }

        public EmailServiceFixture()
        {
            var secretsProvider = new JsonSecretsProvider("appkeys.json");
            EmailService = new SendinblueEmailService(secretsProvider);
        }
    }

    public class EmailServiceTests : IClassFixture<EmailServiceFixture>
    {
        private readonly string TEST_MAIL = "nobihe5151@dukeoo.com";
        private readonly EmailServiceFixture _fixture;

        public EmailServiceTests(EmailServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SendAsync_ValidAddress_Send()
        {
            // Since we are using a temp mail to test this, we don't really have
            // a reliable way to check if the message was received, so the fact that
            // we didn't get any exceptions is enough to pass this test.
            await _fixture.EmailService.SendAsync(TEST_MAIL, "Test", "test message");
        }
    }
}
