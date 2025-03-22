using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesMinimalAPI.DTOs;
using MoviesMinimalAPI.Entities;
using MoviesMinimalAPI.Repositories;
using MoviesMinimalAPI.Services;

namespace MoviesMinimalAPI.Endpoints
{
    public static class MoviesEndpoint
    {
        public static RouteGroupBuilder MapMovies(this RouteGroupBuilder routeGroupBuilder)
        {
            routeGroupBuilder.MapGet("/", GetAll)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("get-all-movies"));
            routeGroupBuilder.MapGet("/{id:int}", GetById);
            routeGroupBuilder.MapPost("/", Create).DisableAntiforgery();
            routeGroupBuilder.MapPut("/{id:int}", Update);
            routeGroupBuilder.MapDelete("/{id:int}", Delete);

            return routeGroupBuilder;
        }

        static async Task<Ok<List<MovieDto>>> GetAll(int take, int skip, IMovieRepository movieRepository, IMapper mapper)
        {
            var movies = await movieRepository.GetAllAsync(take, skip);
            var result = mapper.Map<List<MovieDto>>(movies);

            return TypedResults.Ok(result);
        }

        static async Task<Results<Ok<MovieDto>, NotFound>> GetById(int id, IMovieRepository movieRepository, IMapper mapper)
        {
            var movie = await movieRepository.GetByIdAsync(id);
            var result = mapper.Map<MovieDto>(movie);
            if (result is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }

        static async Task<Created<MovieDto>> Create([FromForm] CreateMovieDto data, IMovieRepository movieRepository, IMapper mapper, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
        {
            var movie = mapper.Map<Movie>(data);
            var id = await movieRepository.CreateAsync(movie);
            if (data.Poster is not null)
            {
                movie.Poster = await fileStorage.StorageFileAsync("movies", data.Poster);
            }
            await outputCacheStore.EvictByTagAsync("get-all-movies", default);
            var movieDto = mapper.Map<MovieDto>(movie);
            return TypedResults.Created($"/movies/{id}", movieDto);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateMovieDto data, IMovieRepository movieRepository, IMapper mapper, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
        {
            var exists = await movieRepository.GetByIdAsync(id);
            if (exists is null)
            {
                return TypedResults.NotFound();
            }

            var movie = mapper.Map<Movie>(data);
            movie.Id = id;
            movie.Poster = exists!.Poster;

            if (data.Poster is not null)
            {
                movie.Poster = await fileStorage.EditFileAsync(exists!.Poster!, "movies", data.Poster);
            }
            await movieRepository.UpdateAsync(movie);
            await outputCacheStore.EvictByTagAsync("get-all-movies", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IMovieRepository movieRepository, IOutputCacheStore outputCacheStore)
        {
            var exists = await movieRepository.IsExistsAsync(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await movieRepository.DeleteAsync(id);
            await outputCacheStore.EvictByTagAsync("get-all-movies", default);
            return TypedResults.NoContent();
        }
    }
}
