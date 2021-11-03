using API.Interfaces;
using API.Models;
using API.Models.Pagination;
using IGDB;
using IGDB.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class IGDBService : IGamesService
    {
        private readonly IGDBClient _igdb;

        public IGDBService()
        {
            var keys = JObject.Parse(File.ReadAllText("appkeys.json"));
            _igdb = new IGDBClient(keys["igdb_client_id"].ToString(), keys["igdb_client_secret"].ToString());
        }

        public async Task<PagedList<VideoGame>> SearchGamesAsync(GamesParams gamesParams)
        {
            if (string.IsNullOrWhiteSpace(gamesParams.Query))
            {
                return PagedList<VideoGame>.CreateEmpty();
            }

            // Make sure the page number is now below 1
            if (gamesParams.PageNumber < 1)
            {
                gamesParams.PageNumber = 1;
            }

            var count = await CountGames(gamesParams);

            // TODO: Add caching
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, 
                $"search \"{gamesParams.Query}\"; fields name,cover.*,release_dates.*; where category = 0; " +
                $"limit {gamesParams.PageSize}; offset {(gamesParams.PageNumber - 1) * gamesParams.PageSize};");

            var games = results.Select(result => ToVideoGame(result));
            return new PagedList<VideoGame>(games, count, gamesParams.PageNumber, gamesParams.PageSize);
        }

        // Counts the games.
        // HACK: This would have been better done using the games/count endpoint but the library doesn't support it,
        // when using games/count as endpoint in the QueryAsync method it will escape the / character.
        private async Task<int> CountGames(GamesParams gamesParams)
        {
            var count = 0;
            Game[] results;

            do
            {
                results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"search \"{gamesParams.Query}\"; limit 500; offset {count};");
                count += results.Length;
            }
            while (results.Length == 500);

            return count;
        }

        public async Task<VideoGame> GetGameAsync(long id)
        {
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, 
                $"fields id,name,cover.*,release_dates.*; where id = {id}; limit 1;");

            if (results.Any())
            {
                var result = results.FirstOrDefault();
                return ToVideoGame(result);
            }

            return null;
        }

        public async Task<VideoGameDetails> GetGameDetailsAsync(long id)
        {
            // TODO: Change this query to get all required fields
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, 
                $"fields id,name,cover.*,release_dates.*,genres.*,summary,screenshots.*; where id = {id}; limit 1;");

            if (results.Any())
            {
                var result = results.FirstOrDefault();
                return ToVideoGameDetails(result);
            }

            return null;
        }

        public string GetImageUrl(string imageId, GameCoverSize size)
        {
            if (string.IsNullOrWhiteSpace(imageId))
            {
                imageId = "nocover";
            }

            var coverSize = size switch
            {
                GameCoverSize.Thumb => ImageSize.Thumb,
                GameCoverSize.Small => ImageSize.CoverSmall,
                GameCoverSize.Big => ImageSize.CoverBig,
                _ => throw new NotImplementedException()
            };

            return "https:" + ImageHelper.GetImageUrl(imageId, coverSize);
        }

        public string GetImageUrl(string screenshotId, GameScreenshotSize size)
        {
            if (string.IsNullOrWhiteSpace(screenshotId))
            {
                return null;
            }

            var screenshotSize = size switch
            {
                GameScreenshotSize.Medium => ImageSize.ScreenshotMed,
                GameScreenshotSize.Big => ImageSize.ScreenshotBig,
                GameScreenshotSize.Huge => ImageSize.ScreenshotHuge,
                _ => throw new NotImplementedException()
            };

            return "https:" + ImageHelper.GetImageUrl(screenshotId, screenshotSize);
        }

        private VideoGame ToVideoGame(Game game)
        {
            if (!game.Id.HasValue)
            {
                return null;
            }

            var videoGame = new VideoGame(game.Id.Value)
            {
                Name = game.Name,
                CoverId = game.Cover?.Value.ImageId
            };

            if (game.ReleaseDates is not null && game.ReleaseDates.Values.Length > 1)
            {
                var releaseDate = game.ReleaseDates.Values.Last();

                if (releaseDate is not null && releaseDate.Year.HasValue)
                {
                    videoGame.Year = releaseDate.Year.Value;
                }
            }

            return videoGame;
        }

        private VideoGameDetails ToVideoGameDetails(Game game)
        {
            if (!game.Id.HasValue)
            {
                return null;
            }

            var videoGame = new VideoGameDetails(game.Id.Value)
            {
                Name = game.Name,
                CoverId = game.Cover?.Value.ImageId,
                Summary = game.Summary,

                Genres = game.Genres is not null
                    ? game.Genres.Values.Select(g => g.Name).ToArray() 
                    : Array.Empty<string>(),

                Screenshots = game.Screenshots is not null
                    ? game.Screenshots.Values.Select(s => ToScreenshot(s)).ToArray()
                    : Array.Empty<VideoGameScreenshot>()
            };

            if (game.ReleaseDates is not null && game.ReleaseDates.Values.Length > 1)
            {
                var releaseDate = game.ReleaseDates.Values.Last();

                if (releaseDate is not null && releaseDate.Year.HasValue)
                {
                    videoGame.Year = releaseDate.Year.Value;
                }
            }

            return videoGame;
        }

        private VideoGameScreenshot ToScreenshot(Screenshot screenshot)
            => new()
            {
                MediumUrl = GetImageUrl(screenshot.ImageId, GameScreenshotSize.Medium),
                BigUrl = GetImageUrl(screenshot.ImageId, GameScreenshotSize.Big),
                HugeUrl = GetImageUrl(screenshot.ImageId, GameScreenshotSize.Huge),
            };
    }
}
