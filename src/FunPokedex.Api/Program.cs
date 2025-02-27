using FunPokedex.Api.Endpoints;
using FunPokedex.Core.Extensions;
using FunPokedex.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

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
builder.Services.AddSwaggerGen(opt =>
{
    opt.MapType<PokemonName>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" });
    opt.MapType<PokemonHabitat>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.RegisterPokemonEndpoints();

app.Run();