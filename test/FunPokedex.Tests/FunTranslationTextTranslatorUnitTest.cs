using FluentAssertions;
using FunPokedex.Clients.FunTranslations;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Contrib.HttpClient;
using System.Text.Json.Nodes;

namespace FunPokedex.Tests;

[TestClass]
public class FunTranslationTextTranslatorUnitTest
{
    private const string ApiBaseUri = "https://api.funtranslations.com/";
    private const string ApiTranslateUri = ApiBaseUri + "translate/{0}?text={1}";

    private readonly Mock<HttpMessageHandler> _mockHttpClientHandler;
    private readonly FunTranslationTextTranslator _sut;

    public FunTranslationTextTranslatorUnitTest()
    {
        _mockHttpClientHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpClientHandler.Object);
        httpClient.BaseAddress = new Uri(ApiBaseUri);
        _sut = new FunTranslationTextTranslator(new NullLogger<FunTranslationTextTranslator>(), httpClient);
    }

    #region [ YodaTranslation ]

    [TestMethod]
    public async Task ApplyYodaTranslationAsync_When_APIReturnsTranslation_Returns_ProperTranslation()
    {
        const string SuccessfullyTranslatedText = "Sample to beest did translate";
        const string SuccessfullJsonStringContentSample = "{\"success\":{\"total\":1},\"contents\":{\"translated\":\"" + SuccessfullyTranslatedText + "\",\"text\":\"sample to be translated\",\"translation\":\"shakespeare\"}}";

        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.YodaTranslatorPath, testString)).ReturnsJsonResponse(JsonObject.Parse(SuccessfullJsonStringContentSample));

        // Act
        var methodResult = await _sut.ApplyYodaTranslationAsync(testString);

        // Assert
        methodResult.Should().Be(SuccessfullyTranslatedText);
    }

    [TestMethod]
    public async Task ApplyYodaTranslationAsync_When_APIReturnsSuccessCountNotEqualToOne_ThrowsApplicationException()
    {
        const string UnsuccessfullJsonStringContentSample = "{\"success\":{\"total\":0},\"contents\":{\"translated\":\"\",\"text\":\"sample to be translated\",\"translation\":\"shakespeare\"}}";

        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.YodaTranslatorPath, testString)).ReturnsJsonResponse(JsonObject.Parse(UnsuccessfullJsonStringContentSample));

        // Act
        await _sut.Invoking(y => y.ApplyYodaTranslationAsync(testString))
                  .Should()
                  .ThrowAsync<ApplicationException>();
    }

    [TestMethod]
    public async Task ApplyYodaTranslationAsync_When_APIReturnsUnsucessfullStatusCode_ThrowsApplicationException()
    {
        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.YodaTranslatorPath, testString)).ReturnsResponse(System.Net.HttpStatusCode.InternalServerError);

        // Act
        await _sut.Invoking(y => y.ApplyYodaTranslationAsync(testString))
                  .Should()
                  .ThrowAsync<HttpRequestException>();

    }

    #endregion

    #region [ ShakespereanTranslation ]

    [TestMethod]
    public async Task ApplyShakespereanTranslationAsync_When_APIReturnsTranslation_Returns_ProperTranslation()
    {
        const string SuccessfullyTranslatedText = "Sample to beest did translate";
        const string SuccessfullJsonStringContentSample = "{\"success\":{\"total\":1},\"contents\":{\"translated\":\"" + SuccessfullyTranslatedText + "\",\"text\":\"sample to be translated\",\"translation\":\"shakespeare\"}}";

        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.ShakespereanTranslatorPath, testString)).ReturnsJsonResponse(JsonObject.Parse(SuccessfullJsonStringContentSample));

        // Act
        var methodResult = await _sut.ApplyShakespereanTranslationAsync(testString);

        // Assert
        methodResult.Should().Be(SuccessfullyTranslatedText);
    }

    [TestMethod]
    public async Task ApplyShakespereanTranslationAsync_When_APIReturnsSuccessCountNotEqualToOne_ThrowsApplicationException()
    {
        const string UnsuccessfullJsonStringContentSample = "{\"success\":{\"total\":0},\"contents\":{\"translated\":\"\",\"text\":\"sample to be translated\",\"translation\":\"shakespeare\"}}";

        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.ShakespereanTranslatorPath, testString)).ReturnsJsonResponse(JsonObject.Parse(UnsuccessfullJsonStringContentSample));

        // Act
        await _sut.Invoking(y => y.ApplyShakespereanTranslationAsync(testString))
                  .Should()
                  .ThrowAsync<ApplicationException>();
    }

    [TestMethod]
    public async Task ApplyShakespereanTranslationAsync_When_APIReturnsUnsucessfullStatusCode_ThrowsApplicationException()
    {
        // Arrange
        var testString = "teststring";
        _mockHttpClientHandler.SetupRequest(HttpMethod.Get, string.Format(ApiTranslateUri, FunTranslationTextTranslator.ShakespereanTranslatorPath, testString)).ReturnsResponse(System.Net.HttpStatusCode.InternalServerError);

        // Act
        await _sut.Invoking(y => y.ApplyShakespereanTranslationAsync(testString))
                  .Should()
                  .ThrowAsync<HttpRequestException>();
    }

    #endregion

}