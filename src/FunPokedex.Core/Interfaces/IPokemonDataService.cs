using FluentResults;
using FunPokedex.Core.Models;
using System.Threading.Tasks;

namespace FunPokedex.Core.Interfaces
{
    public interface IPokemonDataService
    {
        Task<Result<PokemonStandardInfoModel>> GetPokemonStandardInfoAsIsAsync(PokemonName name);
    }
}