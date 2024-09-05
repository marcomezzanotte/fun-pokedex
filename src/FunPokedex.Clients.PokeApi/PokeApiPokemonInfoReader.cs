using FluentResults;
using FunPokedex.Core.Errors;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FunPokedex.Clients.PokeApi;

internal sealed class PokeApiPokemonInfoReader : IPokemonInfoReader
{
    private readonly ILogger<PokeApiPokemonInfoReader> _logger;
    private readonly HttpClient _httpClient;

    public PokeApiPokemonInfoReader(ILogger<PokeApiPokemonInfoReader> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Result<PokemonStandardInfoModel>> GetPokemonByNameAsync(PokemonName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        try
        {
            using (var response = await _httpClient.GetAsync($"pokemon-species/{name.Value}"))
            {
                response.EnsureSuccessStatusCode();
                var pokemonData = await response.Content.ReadFromJsonAsync<PokeApiPokemonData>();
                var resolveDescription = pokemonData.FlavorTextEntries?.FirstOrDefault(x => PokeApiFlavorTextEntryLanguage.EnglishName.Equals(x.Language?.Name, StringComparison.InvariantCultureIgnoreCase)).FlavorText ?? string.Empty;
                return Result.Ok(new PokemonStandardInfoModel(pokemonData.Name, Description: resolveDescription, pokemonData.Habitat.HasValue ? PokemonHabitat.From(pokemonData.Habitat!.Value!.Name!) : null, isLegendary: pokemonData.IsLegendary));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed reading data from PokeApi");
            return Result.Fail(DomainErrors.CannotReadPokemonData);
        }
    }

    private record struct PokeApiPokemonData
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("habitat")]
        public PokeApiHabitat? Habitat { get; init; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; init; }

        [JsonPropertyName("flavor_text_entries")]
        public PokeApiFlavorTextEntry[]? FlavorTextEntries { get; init; }
    }

    private record struct PokeApiFlavorTextEntry
    {
        [JsonPropertyName("flavor_text")]
        public string? FlavorText { get; init; }
        [JsonPropertyName("language")]
        public PokeApiFlavorTextEntryLanguage? Language { get; init; }
    }

    private record struct PokeApiHabitat
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }

    private record struct PokeApiFlavorTextEntryLanguage
    {
        public const string EnglishName = "en";

        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
