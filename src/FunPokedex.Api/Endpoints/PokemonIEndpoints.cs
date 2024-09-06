using FluentResults;
using FunPokedex.Core.Interfaces;
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
        .WithOpenApi();

    }
}
