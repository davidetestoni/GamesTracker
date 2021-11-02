using API.Interfaces;
using API.Models;
using IGDB;
using IGDB.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<VideoGame>> SearchGamesAsync(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return Array.Empty<VideoGame>();
            }

            // TODO: Check if this needs to be sanitized
            // TODO: Add caching
            // TODO: Add pagination: use /games/count endpoint. Then you can use limit and offset in the query to paginate. This is useful
            // since the maximum limit you can set is 500 and if a query returns more than 500 items it's a problem! For now we will return
            // at most 50 items until pagination is implemented.
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, 
                $"search \"{searchString}\"; fields name,cover.*,release_dates.*; where category = 0; limit 50;");
            var games = results.Select(result => ToVideoGame(result));
            return games;
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
                $"fields id,name,cover.*,release_dates.*,genres.*,summary; where id = {id}; limit 1;");

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

        private static VideoGame ToVideoGame(Game game)
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

        private static VideoGameDetails ToVideoGameDetails(Game game)
        {
            if (!game.Id.HasValue)
            {
                return null;
            }

            var videoGame = new VideoGameDetails(game.Id.Value)
            {
                Name = game.Name,
                CoverId = game.Cover?.Value.ImageId,
                Genres = game.Genres is not null ? game.Genres.Values.Select(g => g.Name).ToArray() : Array.Empty<string>(),
                Summary = game.Summary
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
    }
}
