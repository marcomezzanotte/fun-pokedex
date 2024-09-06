using FluentResults;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// In production env setup log destinations in accordance to company and project standards
builder.Logging.ClearProviders().AddSimpleConsole(opt => opt.IncludeScopes = true);

builder.Services.AddProblemDetails();

// uri read and validated from configuration in real-world environments (allow to use non-production api destination in non-production application environments)
builder.Services.AddPokeApiPokemonInfoReader(baseUri: new System.Uri("https://pokeapi.co/api/v2/"));

bool enableSwagger = builder.Environment.IsDevelopment();   // avoid Swagger in production environments (keep application as slim as possible)
if (enableSwagger)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/pokemon/{pokemonName}", async (IPokemonInfoReader reader, string pokemonName) =>
{
    var lookupResult = await reader.GetPokemonByNameAsync(pokemonName);
    return lookupResult switch
    {
        { IsSuccess: true } => Results.Json(lookupResult.Value),
        _ => lookupResult.MapToApiResultsOnFailed()
    };
})
.WithOpenApi();

app.Run();