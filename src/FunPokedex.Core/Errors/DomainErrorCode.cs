using System;

namespace FunPokedex.Core.Errors;

/// <summary>
/// Unique error identifier, defined using a uniform URI format throughout the whole project
/// </summary>
public sealed class DomainErrorCode
{
    private static readonly Uri BaseTypeUri = new Uri("https://project-uri.com/errors/");

    public DomainErrorCode(string errorCodePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorCodePath);
        Value = new Uri(BaseTypeUri, errorCodePath.Trim('/')).ToString();
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is DomainErrorCode casted && casted.Value.Equals(Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
