using FunPokedex.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FunPokedex.Clients.FunTranslations
{
    public sealed class FunTranslationTextTranslator : ITextTranslator
    {
        private const int SuccessfullResponseTotalValue = 1;
        public const string ShakespereanTranslatorPath = "shakespeare.json";
        public const string YodaTranslatorPath = "yoda.json";

        private readonly ILogger<FunTranslationTextTranslator> _logger;
        private HttpClient _httpClient;

        public FunTranslationTextTranslator(ILogger<FunTranslationTextTranslator> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> ApplyShakespereanTranslationAsync(string value)
        {
            return await InternalTranslateThroughAsync(value, ShakespereanTranslatorPath);
        }

        public async Task<string> ApplyYodaTranslationAsync(string value)
        {
            return await InternalTranslateThroughAsync(value, YodaTranslatorPath);
        }

        private async Task<string> InternalTranslateThroughAsync(string value, string translatorPath)
        {
            ArgumentNullException.ThrowIfNull(value);

            using (var response = await _httpClient.GetAsync($"translate/{translatorPath}?text={Uri.EscapeDataString(value)}"))
            {
                response.EnsureSuccessStatusCode();
                var translationResponse = await response.Content.ReadFromJsonAsync<TranslationResponseContent>();
                if (SuccessfullResponseTotalValue.Equals(translationResponse.Success.Total))
                {
                    return translationResponse.Contents.Translated;
                }
                else
                {
                    _logger.LogWarning("Failed transalting data through FunTranslations api: {responseData}", translationResponse);
                    throw new  ApplicationException("Failed transalting data through FunTranslations api");
                }
            }
        }

        private record struct TranslationResponseContent
        {
            [JsonPropertyName("success")]
            public Success Success { get; set; }
            [JsonPropertyName("contents")]
            public Contents Contents { get; set; }
        }

        private record struct Success
        {
            [JsonPropertyName("total")]
            public int? Total { get; set; }
        }

        private record struct Contents
        {
            [JsonPropertyName("translated")]
            public string Translated { get; set; }
        }
    }
}
