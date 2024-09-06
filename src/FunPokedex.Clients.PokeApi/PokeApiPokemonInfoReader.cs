using FluentResults;
using FunPokedex.Core.Errors;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunPokedex.Clients.PokeApi;

internal sealed class PokeApiPokemonInfoReader : IPokemonInfoReader
{
    private const string NonAsciiCharsRegexPattern = "[^\x00 -\x7F]";
    private readonly ILogger<PokeApiPokemonInfoReader> _logger;
    private readonly HttpClient _httpClient;

    public PokeApiPokemonInfoReader(ILogger<PokeApiPokemonInfoReader> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<PokemonStandardInfoModel?> GetPokemonByNameOrDefaultAsync(PokemonName name)
    {
        ArgumentNullException.ThrowIfNull(name);
        using (var response = await _httpClient.GetAsync($"pokemon-species/{name.Value}"))
        {
            // not found case handled separately because it is considered the most frequent "unsuccessfull case" and avoidi exception overhead could be a great benefit for performance
            if (HttpStatusCode.NotFound.Equals(response.StatusCode))
            {
                _logger.LogDebug("No Pokemon found with name {name}", name.Value);
                return default;
            }

            _logger.LogDebug("Successfully retrieved Pokemon data from api");

            response.EnsureSuccessStatusCode();
            var pokemonData = await response.Content.ReadFromJsonAsync<PokeApiPokemonData>();
            var resolveDescription = pokemonData.FlavorTextEntries?.FirstOrDefault(x => PokeApiFlavorTextEntryLanguage.EnglishName.Equals(x.Language?.Name, StringComparison.InvariantCultureIgnoreCase)).FlavorText ?? string.Empty;
            string cleanedDescriptionToTranslate = Regex.Replace(resolveDescription, pattern: NonAsciiCharsRegexPattern, replacement: " ");      // remove non-ASCII chars
            return new PokemonStandardInfoModel(pokemonData.Name, Description: cleanedDescriptionToTranslate, pokemonData.Habitat.HasValue ? PokemonHabitat.From(pokemonData.Habitat!.Value!.Name!) : null, IsLegendary: pokemonData.IsLegendary);
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
