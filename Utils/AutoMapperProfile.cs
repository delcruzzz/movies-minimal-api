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
            CreateMap<Movie, MovieDto>()
                .ForMember(p => p.Genders, entity => entity
                    .MapFrom(p => p.GenderMovies
                        .Select(gp => new GenderDto { Id = gp.GenderId, Name = gp.Gender.Name })))
                .ForMember(p => p.Actors, entity => entity
                    .MapFrom(p => p.ActorMovies
                        .Select(am => new ActorMovieDto { Id = am.ActorId, Name = am.Actor.Name, Character = am.Character })));

            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, CommentDto>();

            CreateMap<ActorMovie, AssignActorMovieDto>().ReverseMap();
        }
    }
}
