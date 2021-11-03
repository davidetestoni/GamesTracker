using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Models;
using AutoMapper;

namespace API.Helpers.Resolvers
{
    public class GameCoverUrlResolver :
        IValueResolver<VideoGame, GameInfoDto, string>, 
        IValueResolver<VideoGameDetails, GameDetailsDto, string>,
        IValueResolver<UserGame, LibraryGameInfoDto, string>
    {
        private readonly IGamesService _gamesService;

        public GameCoverUrlResolver(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public string Resolve(VideoGame source, GameInfoDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.CoverId, GameCoverSize.Big);

        public string Resolve(VideoGameDetails source, GameDetailsDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.CoverId, GameCoverSize.Big);

        public string Resolve(UserGame source, LibraryGameInfoDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.Game.CoverId, GameCoverSize.Big);
    }
}
