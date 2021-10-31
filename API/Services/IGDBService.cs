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

        public async Task<IEnumerable<VideoGame>> SearchGames(string searchString)
        {
            // TODO: Check if this needs to be sanitized
            // TODO: Add caching
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"search \"{searchString}\"; fields name,cover.*;");
            var games = results.Select(r => new VideoGame
            {
                Id = r.Id.Value,
                Name = r.Name,
                CoverUrl = ConvertCover(r)
            });

            return games;
        }

        public async Task<VideoGame> GetGame(int id)
        {
            var results = await _igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, $"fields id,name,cover.*; where id = {id};");

            if (results.Any())
            {
                var result = results.FirstOrDefault();
                return new VideoGame
                {
                    Id = result.Id.Value,
                    Name = result.Name,
                    CoverUrl = ConvertCover(result)
                };
            }

            return null;
        }

        private static string ConvertCover(Game game)
            => $"https:{game.Cover.Value.Url}";
    }
}
