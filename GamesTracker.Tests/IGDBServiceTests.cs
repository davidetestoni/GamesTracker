using API.Interfaces;
using API.Services;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GamesTracker.Tests
{
    public class IGDBFixture
    {
        public IGDBService IGDB { get; set; }
        public HttpClient Http { get; set; }

        public IGDBFixture()
        {
            IGDB = new IGDBService();
            Http = new HttpClient();
        }
    }

    public class IGDBServiceTests : IClassFixture<IGDBFixture>
    {
        private readonly IGDBFixture _fixture;
        private const string VALID_GAME = "Wolfenstein";

        public IGDBServiceTests(IGDBFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SearchGames_ValidGame_NotEmpty()
        {
            var games = await _fixture.IGDB.SearchGames(VALID_GAME);

            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task SearchGames_ValidGame_HasName()
        {
            var games = await _fixture.IGDB.SearchGames(VALID_GAME);
            var game = games.First();

            Assert.True(!string.IsNullOrEmpty(game.Name));
        }

        [Fact]
        public async Task SearchGames_ValidGame_CoverThumbExists()
        {
            var games = await _fixture.IGDB.SearchGames(VALID_GAME);
            var game = games.First();

            var url = _fixture.IGDB.GetCoverUrl(game.CoverId, GameCoverSize.Thumb);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGames_ValidGame_SmallCoverExists()
        {
            var games = await _fixture.IGDB.SearchGames(VALID_GAME);
            var game = games.First();

            var url = _fixture.IGDB.GetCoverUrl(game.CoverId, GameCoverSize.Small);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGames_ValidGame_BigCoverExists()
        {
            var games = await _fixture.IGDB.SearchGames(VALID_GAME);
            var game = games.First();

            var url = _fixture.IGDB.GetCoverUrl(game.CoverId, GameCoverSize.Big);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGames_EmptySearch_EmptyList()
        {
            var games = await _fixture.IGDB.SearchGames(string.Empty);

            Assert.Empty(games);
        }

        [Fact]
        public async Task SearchGames_VeryShortSearch_EmptyList()
        {
            var games = await _fixture.IGDB.SearchGames("a");

            Assert.Empty(games);
        }
    }
}
