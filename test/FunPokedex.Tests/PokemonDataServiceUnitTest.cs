using FluentAssertions;
using FunPokedex.Core.Errors;
using FunPokedex.Core.Interfaces;
using FunPokedex.Core.Models;
using FunPokedex.Core.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FunPokedex.Tests;

[TestClass]
public class PokemonDataServiceUnitTest
{
    private readonly Mock<IPokemonInfoReader> _mockPokemonReader;
    private readonly Mock<ITextTranslator> _mockTextTranslator;
    private readonly PokemonDataService _sut;

    public PokemonDataServiceUnitTest()
    {
        _mockPokemonReader = new Mock<IPokemonInfoReader>(MockBehavior.Strict);
        _mockTextTranslator = new Mock<ITextTranslator>(MockBehavior.Strict);
        _sut = new PokemonDataService(new NullLogger<PokemonDataService>(), _mockPokemonReader.Object, _mockTextTranslator.Object);
    }

    #region [GetPokemonStandardInfoAsIsAsync]

    [TestMethod]
    public async Task GetPokemonStandardInfoAsIsAsync_When_PokeAPIReturnsNull_Returns_UnknownPokemonError()
    {
        // Arrange
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(null as PokemonStandardInfoModel);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoAsIsAsync("unknonw-pokemon");

        // Assert
        methodResult.IsFailed.Should().BeTrue();
        methodResult.HasError<DomainError>().Should().BeTrue();
        methodResult.Errors.Single().Should().BeEquivalentTo(DomainErrors.UnknownPokemon);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoAsIsAsync_When_PokeAPIThrowsException_Returns_CannotReadPokemonDataError()
    {
        // Arrange
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ThrowsAsync(new Exception("sample-pokeapi-exception"));

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoAsIsAsync("any-pokemon");

        // Assert
        methodResult.IsFailed.Should().BeTrue();
        methodResult.HasError<DomainError>().Should().BeTrue();
        methodResult.Errors.Single().Should().BeEquivalentTo(DomainErrors.CannotReadPokemonData);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoAsIsAsync_When_PokeAPIReturnsPokemonData_Returns_CorrespondentPokemonModel()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>().Generate();
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoAsIsAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonData);
    }

    #endregion

    #region [GetPokemonStandardInfoTransaltedAsync]

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_PokeAPIReturnsNull_Returns_UnknownPokemonError()
    {
        // Arrange
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(null as PokemonStandardInfoModel);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("unknonw-pokemon");

        // Assert
        methodResult.IsFailed.Should().BeTrue();
        methodResult.HasError<DomainError>().Should().BeTrue();
        methodResult.Errors.Single().Should().BeEquivalentTo(DomainErrors.UnknownPokemon);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_PokeAPIThrowsException_Returns_CannotReadPokemonDataError()
    {
        // Arrange
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ThrowsAsync(new Exception("sample-pokeapi-exception"));

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsFailed.Should().BeTrue();
        methodResult.HasError<DomainError>().Should().BeTrue();
        methodResult.Errors.Single().Should().BeEquivalentTo(DomainErrors.CannotReadPokemonData);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_CavePokemon_Invokes_YodaTranslation()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>()
            .RuleFor(x => x.Habitat, PokemonHabitat.Cave)
            .Generate();
        PokemonStandardInfoModel samplePokemonDataWithTranslatedDescription = CreateTranslatedDescriptionPokemonData(samplePokemonData);
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);
        _mockTextTranslator.Setup(x => x.ApplyYodaTranslationAsync(It.IsAny<string>())).ReturnsAsync(samplePokemonDataWithTranslatedDescription.Description);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonDataWithTranslatedDescription);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_IsLegendary_Invokes_YodaTranslation()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>()
            .RuleFor(x => x.IsLegendary, true)
            .Generate();
        PokemonStandardInfoModel samplePokemonDataWithTranslatedDescription = CreateTranslatedDescriptionPokemonData(samplePokemonData);
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);
        _mockTextTranslator.Setup(x => x.ApplyYodaTranslationAsync(It.IsAny<string>())).ReturnsAsync(samplePokemonDataWithTranslatedDescription.Description);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonDataWithTranslatedDescription);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_CavePokemonAndIsLegendary_Invokes_YodaTranslation()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>()
            .RuleFor(x => x.Habitat, PokemonHabitat.Cave)
            .RuleFor(x => x.IsLegendary, true)
            .Generate();
        PokemonStandardInfoModel samplePokemonDataWithTranslatedDescription = CreateTranslatedDescriptionPokemonData(samplePokemonData);
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);
        _mockTextTranslator.Setup(x => x.ApplyYodaTranslationAsync(It.IsAny<string>())).ReturnsAsync(samplePokemonDataWithTranslatedDescription.Description);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonDataWithTranslatedDescription);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_NonCavePokemonAndNonLegendary_Invokes_ShakespereanTranslation()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>()
            .RuleFor(x => x.Habitat, "any-habitat-other-than-cave")
            .RuleFor(x => x.IsLegendary, false)
            .Generate();
        PokemonStandardInfoModel samplePokemonDataWithTranslatedDescription = CreateTranslatedDescriptionPokemonData(samplePokemonData);
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);
        _mockTextTranslator.Setup(x => x.ApplyShakespereanTranslationAsync(It.IsAny<string>())).ReturnsAsync(samplePokemonDataWithTranslatedDescription.Description);

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonDataWithTranslatedDescription);
    }

    [TestMethod]
    public async Task GetPokemonStandardInfoTransaltedAsync_When_FunTranslationsThrows_Returns_CorrespondentPokemonModelWithOriginalDescription()
    {
        // Arrange
        var samplePokemonData = new AutoBogus.AutoFaker<PokemonStandardInfoModel>()
                       .RuleFor(x => x.Habitat, PokemonHabitat.Cave)
                       .RuleFor(x => x.IsLegendary, true)
                       .Generate();
        _mockPokemonReader.Setup(x => x.GetPokemonByNameOrDefaultAsync(It.IsAny<PokemonName>())).ReturnsAsync(samplePokemonData);
        _mockTextTranslator.Setup(x => x.ApplyYodaTranslationAsync(It.IsAny<string>())).ThrowsAsync(new Exception("sample-pokeapi-exception"));

        // Act
        var methodResult = await _sut.GetPokemonStandardInfoTransaltedAsync("any-pokemon");

        // Assert
        methodResult.IsSuccess.Should().BeTrue();
        methodResult.Value.Should().BeEquivalentTo(samplePokemonData);
    }

    #endregion

    private static PokemonStandardInfoModel CreateTranslatedDescriptionPokemonData(PokemonStandardInfoModel samplePokemonData)
    {
        var translatedDescription = $"{samplePokemonData.Description}-translated";
        return new PokemonStandardInfoModel(Name: samplePokemonData.Name, Description: translatedDescription, Habitat: samplePokemonData.Habitat, IsLegendary: samplePokemonData.IsLegendary);
    }
}