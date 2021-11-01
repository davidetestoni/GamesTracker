using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;

namespace API.Helpers.Resolvers
{
    public class GameThumbUrlResolver : IValueResolver<VideoGame, GameDto, string>
    {
        private readonly IGamesService _gamesService;

        public GameThumbUrlResolver(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public string Resolve(VideoGame source, GameDto destination, string destMember, ResolutionContext context)
            => _gamesService.GetImageUrl(source.CoverId, GameCoverSize.Thumb);
    }
}
