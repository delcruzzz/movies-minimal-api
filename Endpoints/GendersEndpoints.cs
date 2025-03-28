using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.OutputCaching;
using MoviesMinimalAPI.DTOs;
using MoviesMinimalAPI.Entities;
using MoviesMinimalAPI.Repositories;

namespace MoviesMinimalAPI.Endpoints
{
    public static class GendersEndpoints
    {
        public static RouteGroupBuilder MapGenders(this RouteGroupBuilder routeGroupBuilder)
        {
            routeGroupBuilder.MapGet("/", async (IGenderRepository genderRepository) =>
            {
                var genders = await genderRepository.GetAllAsync();
                return Results.Ok(genders);
            }).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)).Tag("get-all-genders"));

            // endpoint for get gender by id
            routeGroupBuilder.MapGet("/{id:int}", async (int id, IGenderRepository genderRepository) =>
            {
                var gender = await genderRepository.GetByIdAsync(id);

                if (gender is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(gender);
            });

            // endpoint for post gender
            routeGroupBuilder.MapPost("/", async (CreateGenderDto data, IGenderRepository genderRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IValidator<CreateGenderDto> validator) =>
            {
                var validationResults = await validator.ValidateAsync(data);

                if (!validationResults.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResults.ToDictionary());
                }

                var gender = mapper.Map<Gender>(data);

                var id = await genderRepository.CreateAsync(gender);
                await outputCacheStore.EvictByTagAsync("get-all-genders", default);
                return Results.Created($"/genders/{id}", data);
            });

            routeGroupBuilder.MapPut("/{id:int}", async (int id, CreateGenderDto data, IGenderRepository genderRepository, IOutputCacheStore outputCacheStore, IMapper mapper, IValidator<CreateGenderDto> validator) =>
            {
                var validationResults = await validator.ValidateAsync(data);

                if (!validationResults.IsValid)
                {
                    return TypedResults.ValidationProblem(validationResults.ToDictionary());
                }

                var exists = await genderRepository.IsExistsAsync(id);

                if (!exists)
                {
                    return Results.NotFound();
                }

                var gender = mapper.Map<Gender>(data);

                await genderRepository.UpdateAsync(gender);
                await outputCacheStore.EvictByTagAsync("get-all-genders", default);
                return Results.NoContent();
            });

            routeGroupBuilder.MapDelete("/{id:int}", async (int id, IGenderRepository genderRepository, IOutputCacheStore outputCacheStore) =>
            {
                var esists = await genderRepository.IsExistsAsync(id);

                if (!esists)
                {
                    return Results.NotFound();
                }

                await genderRepository.DeleteAsync(id);
                await outputCacheStore.EvictByTagAsync("get-all-genders", default);
                return Results.NoContent();
            });

            return routeGroupBuilder;
        }
    }
}
