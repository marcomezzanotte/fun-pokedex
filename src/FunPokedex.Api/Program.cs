using FluentResults;
using FunPokedex.Core.Extensions;
using FunPokedex.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// In production env setup log destinations in accordance to company and project standards
builder.Logging.ClearProviders().AddSimpleConsole(opt => opt.IncludeScopes = true);

builder.Services.AddProblemDetails();

builder.Services.AddCoreServices();

// uri read and validated from configuration in real-world environments (allow to use non-production api destination in non-production application environments)
builder.Services.AddPokeApiPokemonInfoReader(baseUri: new System.Uri("https://pokeapi.co/api/v2/"));
builder.Services.AddFunTranslationsTextTranslator(baseUri: new System.Uri("https://api.funtranslations.com"));

// avoid Swagger in production environments (keep application as slim as possible)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/pokemon/{pokemonName}", async (IPokemonDataService pokemonDataService, string pokemonName) =>
{
    var serviceResult = await pokemonDataService.GetPokemonStandardInfoAsIsAsync(pokemonName);
    return serviceResult switch
    {
        { IsSuccess: true } => Results.Json(serviceResult.Value),
        _ => serviceResult.MapToApiResultsOnFailed()
    };
})
.WithOpenApi();

app.MapGet("/pokemon/translated/{pokemonName}", async (IPokemonDataService pokemonDataService, string pokemonName) =>
{
    var serviceResult = await pokemonDataService.GetPokemonStandardInfoTransaltedAsync(pokemonName);
    return serviceResult switch
    {
        { IsSuccess: true } => Results.Json(serviceResult.Value),
        _ => serviceResult.MapToApiResultsOnFailed()
    };
})
.WithOpenApi();

app.Run();