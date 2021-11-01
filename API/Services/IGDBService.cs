using API.Interfaces;
using API.Models;
using IGDB;
using IGDB.Models;
using Newtonsoft.Json.Linq;
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
            // TODO: Check if this needs to be sanitized
            // TODO: Add caching
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"search \"{searchString}\"; fields name,cover.*; limit 100;");
            var games = results.Select(result => ToVideoGame(result));
            return games;
        }

        public async Task<VideoGame> GetGameAsync(long id)
        {
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"fields id,name,cover.*; where id = {id}; limit 1;");

            if (results.Any())
            {
                var result = results.FirstOrDefault();
                return ToVideoGame(result);
            }

            return null;
        }

        public string GetImageUrl(string imageId, GameCoverSize size)
        {
            if (string.IsNullOrWhiteSpace(imageId))
            {
                return null;
            }

            var coverSize = size switch
            {
                GameCoverSize.Thumb => ImageSize.Thumb,
                GameCoverSize.Small => ImageSize.CoverSmall,
                GameCoverSize.Big => ImageSize.CoverBig,
                _ => throw new System.NotImplementedException()
            };

            return "https:" + ImageHelper.GetImageUrl(imageId, coverSize);
        }

        private static VideoGame ToVideoGame(Game game)
        {
            if (!game.Id.HasValue)
            {
                return null;
            }

            return new VideoGame(game.Id.Value)
            {
                Name = game.Name,
                CoverId = game.Cover?.Value.ImageId
            };
        }
    }
}
