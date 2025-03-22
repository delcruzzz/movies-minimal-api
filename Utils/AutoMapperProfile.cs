using AutoMapper;
using MoviesMinimalAPI.DTOs;
using MoviesMinimalAPI.Entities;

namespace MoviesMinimalAPI.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Gender, CreateGenderDto>().ReverseMap();
            CreateMap<Gender, GenderDto>();

            CreateMap<Actor, CreateActorDto>().ReverseMap()
                .ForMember(x => x.Photo, options => options.Ignore()); // ignore the Photo property when mapping from CreateActorDto to Actor
            CreateMap<Actor, ActorDto>();
            
            CreateMap<Movie, CreateMovieDto>().ReverseMap()
                .ForMember(x => x.Poster, options => options.Ignore()); // ignore the Poster property when mapping from CreateMovieDto to Movie
            CreateMap<Movie, MovieDto>();
        }
    }
}
