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
    public static class ActorsEndpoint
    {
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder routeGroupBuilder)
        {
            routeGroupBuilder.MapPost("/", Create).DisableAntiforgery();
            routeGroupBuilder.MapGet("/", GetAll)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("get-all-actors"));
            routeGroupBuilder.MapGet("/{id:int}", GetById);
            routeGroupBuilder.MapPut("/{id:int}", Update);
            routeGroupBuilder.MapDelete("/{id:int}", Delete);

            return routeGroupBuilder;
        }

        static async Task<Ok<List<ActorDto>>> GetAll(int take, int skip, IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var actors = await actorRepository.GetAllAsync(take, skip);
            var result = mapper.Map<List<ActorDto>>(actors);

            return TypedResults.Ok(result);
        }

        static async Task<Results<Ok<ActorDto>, NotFound>> GetById(int id, IActorRepository actorRepository, IMapper mapper)
        {
            var actor = await actorRepository.GetByIdAsync(id);
            var result = mapper.Map<ActorDto>(actor);

            if (result is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(result);
        }

        static async Task<Created<ActorDto>> Create([FromForm] CreateActorDto data, IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(data);
            var id = await actorRepository.CreateAsync(actor);

            if (data.Photo is not null)
            {
                actor.Photo = await fileStorage.StorageFileAsync("actors", data.Photo); 
            }

            await outputCacheStore.EvictByTagAsync("get-all-actors", default);
            var actorDto = mapper.Map<ActorDto>(actor);
            return TypedResults.Created($"/actors/{id}", actorDto);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CreateActorDto data, IActorRepository actorRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
        {
            var exists = await actorRepository.GetByIdAsync(id);
            if (exists is null)
            {
                TypedResults.NotFound();
            }

            var actor = mapper.Map<Actor>(data);
            actor.Id = id;
            actor.Photo = exists!.Photo;
            
            if (data.Photo is not null)
            {
                actor.Photo = await fileStorage.EditFileAsync(exists!.Photo!, "actors", data.Photo!);
            }
            await actorRepository.UpdateAsync(actor);
            await outputCacheStore.EvictByTagAsync("get-all-actors", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id, IActorRepository actorRepository, IOutputCacheStore outputCacheStore)
        {
            var exists = await actorRepository.IsExistsAsync(id);
            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await actorRepository.DeleteAsync(id);
            await outputCacheStore.EvictByTagAsync("get-all-actors", default);
            return TypedResults.NoContent();
        }
    }
}
