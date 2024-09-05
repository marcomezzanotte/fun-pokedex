namespace FunPokedex.Core.Errors;

/// <summary>
/// Classify in a generic and API-friendly manner the "reason" of the error
/// </summary>
public enum DomainErrorStatuses
{
    NotFound,
    InvalidRequest,
    Failed,
    Forbidden,
    Conflict,
    DependencyError
}
