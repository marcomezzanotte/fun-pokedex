using FunPokedex.Clients.PokeApi;
using FunPokedex.Core.Interfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class FunPokedexClientPokeApiServiceCollectionExtensions
{
    public static void AddPokeApiPokemonInfoReader(this IServiceCollection services, Uri baseUri)
    {
        services.AddHttpClient<IPokemonInfoReader, PokeApiPokemonInfoReader>(opt => opt.BaseAddress = baseUri).AddStandardResilienceHandler();
    }
}
