using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;

namespace API.Helpers.Resolvers
{
    public class GameCoverUrlResolver :
        IValueResolver<VideoGame, GameInfoDto, string>, 
        IValueResolver<VideoGame, GameDetailsDto, string>
    {
        private readonly IGamesService _gamesService;

        public GameCoverUrlResolver(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public string Resolve(VideoGame source, GameInfoDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.CoverId, GameCoverSize.Big);

        public string Resolve(VideoGame source, GameDetailsDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.CoverId, GameCoverSize.Big);
    }
}
