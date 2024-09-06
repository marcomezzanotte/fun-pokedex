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

        public PokemonDataService(ILogger<PokemonDataService> logger, IPokemonInfoReader pokemonInfoReader)
        {
            _logger = logger;
            _pokemonInfoReader = pokemonInfoReader;
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
    }
}
