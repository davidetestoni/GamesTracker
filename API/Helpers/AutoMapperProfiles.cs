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
        }
    }
}
