using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
