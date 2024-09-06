using FunPokedex.Api.Errors;
using FunPokedex.Core.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults;

public static class FunPokedexFluentResultExtensions
{
    public static IResult MapToApiResultsOnFailed(this Result failedDomainResult)
    {
        ArgumentNullException.ThrowIfNull(failedDomainResult);

        if (failedDomainResult.HasError<DomainError>(out IEnumerable<DomainError> domainErrors))
        {
            var targetError = domainErrors.First(); //return only first error as API response
            var statusCode = MapStatusCode(targetError);
            return Results.Problem(statusCode: statusCode, title: targetError.Message, type: targetError.Code.Value, extensions: targetError.Metadata);
        }
        else if (failedDomainResult.IsFailed)
        {
            return Results.Problem(ApiErrors.UnexpectedError);
        }

        throw new ArgumentException("Only unsuccessfull results can be handled");
    }

    public static IResult MapToApiResultsOnFailed<TValue>(this Result<TValue> failedDomainResult)
    {
        ArgumentNullException.ThrowIfNull(failedDomainResult);

        if (failedDomainResult.HasError<DomainError>(out IEnumerable<DomainError> domainErrors))
        {
            var targetError = domainErrors.First(); //return only first error as API response
            var statusCode = MapStatusCode(targetError);
            return Results.Problem(statusCode: statusCode, title: targetError.Message, type: targetError.Code.Value, extensions: targetError.Metadata);
        }
        else if (failedDomainResult.IsFailed)
        {
            return Results.Problem(ApiErrors.UnexpectedError);
        }

        throw new ArgumentException("Only unsuccessfull results can be handled");
    }

    private static int MapStatusCode(DomainError targetError)
    {
        return targetError.Status switch
        {
            DomainErrorStatuses.NotFound => StatusCodes.Status404NotFound,
            DomainErrorStatuses.InvalidRequest => StatusCodes.Status400BadRequest,
            DomainErrorStatuses.Forbidden => StatusCodes.Status403Forbidden,
            DomainErrorStatuses.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
