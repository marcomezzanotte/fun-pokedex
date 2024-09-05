using System;
using Vogen;

namespace FunPokedex.Core.Models;

/// <summary>
/// Represents the "Pokemon name"  domain object.
/// A typed string class is used because the Pokemon API treats the name always lowercase and this class allows the necessary cleanup to any string that should be considered a Pokemon name.
/// In this kind of domain, the Pokemon name could be considered more similar to and identifier than a "regular string".
/// </summary>
[ValueObject<string>(Conversions.SystemTextJson, fromPrimitiveCasting: CastOperator.Implicit)]
public sealed partial class PokemonName
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