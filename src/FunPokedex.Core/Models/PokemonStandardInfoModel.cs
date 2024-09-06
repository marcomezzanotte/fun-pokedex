namespace FunPokedex.Core.Models;

public sealed record PokemonStandardInfoModel(PokemonName Name, string Description, PokemonHabitat? Habitat, bool IsLegendary)
{
    public bool ShouldApplyYodaTranslation()
    {
        return IsLegendary || PokemonHabitat.Cave.Equals(Habitat);
    }
}
