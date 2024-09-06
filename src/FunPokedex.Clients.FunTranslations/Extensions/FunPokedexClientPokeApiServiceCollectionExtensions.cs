using FunPokedex.Clients.FunTranslations;
using FunPokedex.Core.Interfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class FunPokedexClientFunTranslationsServiceCollectionExtensions
{
    public static void AddFunTranslationsTextTranslator(this IServiceCollection services, Uri baseUri)
    {
        services.AddHttpClient<ITextTranslator, FunTranslationTextTranslator>(opt => opt.BaseAddress = baseUri).AddStandardResilienceHandler();
    }
}
