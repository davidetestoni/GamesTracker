using API.Interfaces;
using API.Models.Pagination;
using API.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
            var opts = Options.Create(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(opts);
            IGDB = new IGDBService(cache);
            Http = new HttpClient();
        }
    }

    public class IGDBServiceTests : IClassFixture<IGDBFixture>
    {
        private readonly IGDBFixture _fixture;
        private readonly string VALID_GAME = "Wolfenstein";

        public IGDBServiceTests(IGDBFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SearchGamesAsync_ValidGame_NotEmpty()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(VALID_GAME));

            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task SearchGamesAsync_ValidGame_HasName()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(VALID_GAME));
            var game = games.First();

            Assert.True(!string.IsNullOrEmpty(game.Name));
        }

        [Fact]
        public async Task SearchGamesAsync_ValidGame_CoverThumbExists()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(VALID_GAME));
            var game = games.First();

            var url = _fixture.IGDB.GetImageUrl(game.CoverId, GameCoverSize.Thumb);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGamesAsync_ValidGame_SmallCoverExists()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(VALID_GAME));
            var game = games.First();

            var url = _fixture.IGDB.GetImageUrl(game.CoverId, GameCoverSize.Small);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGamesAsync_ValidGame_BigCoverExists()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(VALID_GAME));
            var game = games.First();

            var url = _fixture.IGDB.GetImageUrl(game.CoverId, GameCoverSize.Big);
            var bytes = await _fixture.Http.GetByteArrayAsync(url);

            Assert.NotEmpty(bytes);
        }

        [Fact]
        public async Task SearchGamesAsync_EmptySearch_EmptyList()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf(string.Empty));

            Assert.Empty(games);
        }

        [Fact]
        public async Task SearchGamesAsync_VeryShortSearch_EmptyList()
        {
            var games = await _fixture.IGDB.SearchGamesAsync(FirstPageOf("a"));

            Assert.Empty(games);
        }

        [Fact]
        public async Task SearchGamesAsync_FirstPage_GetPaginatedResults()
        {
            var gamesParams = new GamesParams
            {
                Query = VALID_GAME,
                PageNumber = 1,
                PageSize = 10
            };

            var games = await _fixture.IGDB.SearchGamesAsync(gamesParams);

            Assert.Equal(10, games.Count);
        }

        [Fact]
        public async Task SearchGamesAsync_SecondPage_GetDifferentResults()
        {
            var gamesParams = new GamesParams
            {
                Query = VALID_GAME,
                PageNumber = 1,
                PageSize = 10
            };

            var firstPageGames = await _fixture.IGDB.SearchGamesAsync(gamesParams);
            
            gamesParams.PageNumber = 2;
            var secondPageGames = await _fixture.IGDB.SearchGamesAsync(gamesParams);

            Assert.NotEqual(firstPageGames[0].Id, secondPageGames[0].Id);
        }

        [Fact]
        public async Task SearchGamesAsync_PageZero_GetFirstPage()
        {
            var gamesParams = new GamesParams
            {
                Query = VALID_GAME,
                PageNumber = 0,
                PageSize = 10
            };

            var zeroPageGames = await _fixture.IGDB.SearchGamesAsync(gamesParams);

            gamesParams.PageNumber = 1;
            var firstPageGames = await _fixture.IGDB.SearchGamesAsync(gamesParams);

            Assert.Equal(zeroPageGames[0].Id, firstPageGames[0].Id);
        }

        [Fact]
        public async Task SearchGamesAsync_BigPageNumber_EmptyList()
        {
            var gamesParams = new GamesParams
            {
                Query = VALID_GAME,
                PageNumber = 1000,
                PageSize = 10
            };

            var games = await _fixture.IGDB.SearchGamesAsync(gamesParams);

            Assert.Empty(games);
        }

        private static GamesParams FirstPageOf(string query)
            => new() { Query = query, PageSize = 50, PageNumber = 1 };
    }
}
