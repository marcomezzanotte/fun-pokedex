using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FunPokedex.Core.Extensions
{
    public static class FunPokedexCoreServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IPokemonDataService, PokemonDataService>();
        }
    }
}
