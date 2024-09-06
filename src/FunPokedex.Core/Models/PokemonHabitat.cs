using Vogen;

namespace FunPokedex.Core.Models;

/// <summary>
/// Represents the Pokemons' habitat  domain object.
/// In this kind of domain, the "Pokemon habitat string" could be considered more similar to and identifier than a "regular string".
/// </summary>
[ValueObject<string>(Conversions.SystemTextJson, fromPrimitiveCasting: CastOperator.Implicit)]
[Instance("Cave","cave","Cave habitat value instance")]
public sealed partial class PokemonHabitat
{
    public static Validation Validate(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? Validation.Invalid("Cannot be null or whitespace") : Validation.Ok;
    }

    private static string NormalizeInput(string input)
    {
        return input.Trim().ToLowerInvariant();
    }
}