using FluentResults;
using System;

namespace FunPokedex.Core.Errors;

/// <summary>
/// FluentResult implementation of application's domain error: it has its own identifier <see cref="Code"/> in order to be properly handled by the application
/// </summary>
public sealed class DomainError : Error
{
    public DomainError(string message, DomainErrorCode code, DomainErrorStatuses status) : base(message)
    {
        ArgumentNullException.ThrowIfNull(code);
        Code = code;
        Status = status;
    }

    public DomainErrorCode Code { get; }
    public DomainErrorStatuses Status { get; }

    public override string ToString()
    {
        return $"Domain error {Code} - {Status}";
    }

    public override bool Equals(object? obj)
    {
        return obj is DomainError casted && casted.Code.Equals(Code);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}
