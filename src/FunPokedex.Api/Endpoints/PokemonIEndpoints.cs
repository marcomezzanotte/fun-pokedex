using FluentResults;
using FunPokedex.Core.Errors;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FunPokedex.Api.Endpoints;

public static class PokemonEndpoints
{
    public static void RegisterPokemonEndpoints(this IEndpointRouteBuilder builder)
    {
        var groupBuilder = builder.MapGroup("pokemon");

        groupBuilder.MapGet("/{pokemonName}", async (IPokemonDataService pokemonDataService, string pokemonName) =>
        {
            var serviceResult = await pokemonDataService.GetPokemonStandardInfoAsIsAsync(pokemonName);
            return serviceResult switch
            {
                { IsSuccess: true } => Results.Json(serviceResult.Value),
                _ => serviceResult.MapToApiResultsOnFailed()
            };
        })
        .WithDescription("Returns standard Pokemon data having requested name")
        .Produces<PokemonStandardInfoModel>()
        .ProducesProblem(DomainErrors.UnknownPokemon.MapStatusCode())
        .ProducesProblem(DomainErrors.CannotReadPokemonData.MapStatusCode())
        .WithOpenApi();

        groupBuilder.MapGet("/translated/{pokemonName}", async (IPokemonDataService pokemonDataService, string pokemonName) =>
        {
            var serviceResult = await pokemonDataService.GetPokemonStandardInfoTransaltedAsync(pokemonName);
            return serviceResult switch
            {
                { IsSuccess: true } => Results.Json(serviceResult.Value),
                _ => serviceResult.MapToApiResultsOnFailed()
            };
        })
        .WithDescription("Returns standard Pokemon data having requested name with attempting to translate the description with a funny one")
        .Produces<PokemonStandardInfoModel>()
        .ProducesProblem(DomainErrors.UnknownPokemon.MapStatusCode())
        .ProducesProblem(DomainErrors.CannotReadPokemonData.MapStatusCode())
        .WithOpenApi();

    }
}
