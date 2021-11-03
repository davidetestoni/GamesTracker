using API.DTOs;
using API.Entities;
using API.Helpers.Resolvers;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, UserInfoDto>();
            
            CreateMap<VideoGame, GameDto>()
                .ForMember(dest => dest.ThumbUrl, opt => opt.MapFrom<GameThumbUrlResolver>());
            
            CreateMap<VideoGame, GameInfoDto>()
                .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom<GameCoverUrlResolver>());

            CreateMap<VideoGameDetails, GameDetailsDto>()
                .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom<GameCoverUrlResolver>());

            CreateMap<VideoGameScreenshot, ScreenshotDto>();

            CreateMap<RegisterDto, AppUser>();

            CreateMap<UserGame, LibraryGameInfoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Game.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Game.Year))
                .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom<GameCoverUrlResolver>());

            CreateMap<VideoGame, Game>();
        }
    }
}
