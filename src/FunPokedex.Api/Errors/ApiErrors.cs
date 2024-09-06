using FunPokedex.Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FunPokedex.Api.Errors;

/// <summary>
/// Enumeration of all possibile errors occured at "presentation/api" level
/// </summary>
public static class ApiErrors
{
    public static readonly ProblemDetails UnexpectedError = new ProblemDetails
    {
        Type = new Uri(DomainErrorCode.BaseTypeUri, "api/unexpected-error").ToString(),
        Status = StatusCodes.Status500InternalServerError,
        Title = "Unexpected error"
    };
}
