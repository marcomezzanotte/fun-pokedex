namespace FunPokedex.Core.Models;

public sealed record PokemonStandardInfoModel(PokemonName Name, string Description, PokemonHabitat? Habitat, bool isLegendary);
