using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MoviesMinimalAPI.DTOs;
using MoviesMinimalAPI.Entities;
using MoviesMinimalAPI.Repositories;

namespace MoviesMinimalAPI.Endpoints
{
    public static class CommentsEndpoint
    {
        public static RouteGroupBuilder MapComments(this RouteGroupBuilder routeGroupBuilder)
        {
            routeGroupBuilder.MapPost("/", Create);
            routeGroupBuilder.MapGet("/", GetAll)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("get-all-comments"));
            routeGroupBuilder.MapGet("/{id:int}", GetById);
            routeGroupBuilder.MapGet("/movies/{movieId:int}", GetAllByMovie);
            routeGroupBuilder.MapPut("/{id:int}", Update);
            routeGroupBuilder.MapDelete("/{id:int}", Delete);

            return routeGroupBuilder;
        }

        public static async Task<Results<Created<CommentDto>, BadRequest>> Create(CreateCommentDto data, ICommentRepository commentRepository, IMapper mapper, IOutputCacheStore outputCacheStore, IMovieRepository movieRepository)
        {
            if (! await movieRepository.IsExistsAsync(data.MovieId))
            {
                return TypedResults.BadRequest();
            }

            var comment = mapper.Map<Comment>(data);
            var id = await commentRepository.CreateAsync(comment);
            await outputCacheStore.EvictByTagAsync("get-all-comments", default);
            var commentDto = mapper.Map<CommentDto>(comment);
            return TypedResults.Created($"/comments/{id}", commentDto);
        }

        public static async Task<Ok<List<CommentDto>>> GetAll(ICommentRepository commentRepository, IMapper mapper)
        {
            var comments = await commentRepository.GetAllAsync();
            var result = mapper.Map<List<CommentDto>>(comments);

            return TypedResults.Ok(result);
        }

        public static async Task<Results<Ok<List<CommentDto>>, NotFound>> GetAllByMovie(int movieId, ICommentRepository commentRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            if (! await movieRepository.IsExistsAsync(movieId))
            {
                return TypedResults.NotFound();
            }

            var comments = await commentRepository.GetAllByMovieAsync(movieId);
            var result = mapper.Map<List<CommentDto>>(comments);
            return TypedResults.Ok(result);
        }

        public static async Task<Results<Ok<CommentDto>, NotFound>> GetById(int id, ICommentRepository commentRepository, IMapper mapper)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            var result = mapper.Map<CommentDto>(comment);
            if (result is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(result);
        }

        public static async Task<Results<NoContent, BadRequest, NotFound>> Update(int id, CreateCommentDto data, ICommentRepository commentRepository, IMapper mapper, IMovieRepository movieRepository, IOutputCacheStore outputCacheStore)
        {
            if (! await commentRepository.IsExistsAsync(id))
            {
                return TypedResults.NotFound();
            }

            if (! await movieRepository.IsExistsAsync(data.MovieId))
            {
                return TypedResults.BadRequest();
            }

            var comment = mapper.Map<Comment>(data);
            comment.Id = id;
            
            await commentRepository.UpdateAsync(comment);
            await outputCacheStore.EvictByTagAsync("get-all-comments", default);

            return TypedResults.NoContent();
        }

        public static async Task<Results<NoContent, NotFound>> Delete(int id, ICommentRepository commentRepository, IOutputCacheStore outputCacheStore)
        {
            if (! await commentRepository.IsExistsAsync(id))
            {
                TypedResults.NotFound();
            }

            await commentRepository.DeleteAsync(id);
            await outputCacheStore.EvictByTagAsync("get-all-comments", default);
            return TypedResults.NoContent();
        }
    }
}