using FluentResults;
using FunPokedex.Core.Errors;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunPokedex.Core.Services
{
    internal sealed class PokemonDataService : IPokemonDataService
    {
        private readonly ILogger<PokemonDataService> _logger;
        private readonly IPokemonInfoReader _pokemonInfoReader;
        private readonly ITextTranslator _textTranslator;

        public PokemonDataService(ILogger<PokemonDataService> logger, IPokemonInfoReader pokemonInfoReader, ITextTranslator textTranslator)
        {
            _logger = logger;
            _pokemonInfoReader = pokemonInfoReader;
            _textTranslator = textTranslator;
        }

        public async Task<Result<PokemonStandardInfoModel>> GetPokemonStandardInfoAsIsAsync(PokemonName name)
        {
            try
            {
                var pokemonDataResult = await _pokemonInfoReader.GetPokemonByNameOrDefaultAsync(name);
                return pokemonDataResult is null ? Result.Fail(DomainErrors.UnknownPokemon) : Result.Ok(pokemonDataResult);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed reading data from PokeApi");
                return Result.Fail(DomainErrors.CannotReadPokemonData);
            }
        }

        public async Task<Result<PokemonStandardInfoModel>> GetPokemonStandardInfoTransaltedAsync(PokemonName name)
        {
            using (_logger.BeginScope("Obtaining Pokemon {pokemonName} data and translate description", name))
            {
                // lookup data as-is
                var lookupResult = await GetPokemonStandardInfoAsIsAsync(name);
                if (!lookupResult.IsSuccess)
                {
                    return lookupResult;
                }

                _logger.LogInformation("Successfully read Pokemon data. Translating description");

                // on successfull lookup, proceed with translation
                try
                {
                    var pokemonDataAsIs = lookupResult.Value;
                    string translatedDescription;
                    if (pokemonDataAsIs.ShouldApplyYodaTranslation())
                    {
                        _logger.LogDebug("Apply Yoda translation");
                        translatedDescription = await _textTranslator.ApplyYodaTranslationAsync(pokemonDataAsIs.Description);
                    }
                    else
                    {
                        _logger.LogDebug("Apply Shakespearean translation");
                        translatedDescription = await _textTranslator.ApplyShakespereanTranslationAsync(pokemonDataAsIs.Description);
                    }

                    _logger.LogInformation("Successfully translated Pokemon data");

                    return Result.Ok(new PokemonStandardInfoModel(pokemonDataAsIs.Name, translatedDescription, pokemonDataAsIs.Habitat, pokemonDataAsIs.IsLegendary));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed transalting data through FunTranslations");
                    return Result.Fail(DomainErrors.CannotTranslatePokemonData);
                }
            }
        }
    }
}
