using FluentResults;
using FunPokedex.Core.Models;
using System.Threading.Tasks;

namespace FunPokedex.Core.Interfaces;

public interface IPokemonInfoReader
{
    Task<Result<PokemonStandardInfoModel>> GetPokemonByNameAsync(PokemonName name);
}
