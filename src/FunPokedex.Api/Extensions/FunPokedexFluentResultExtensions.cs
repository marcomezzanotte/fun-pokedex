using FunPokedex.Api.Errors;
using FunPokedex.Core.Errors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults;

public static class FunPokedexFluentResultExtensions
{
    public static IResult MapToApiResultsOnFailed(this ResultBase failedDomainResult)
    {
        ArgumentNullException.ThrowIfNull(failedDomainResult);

        if (failedDomainResult.HasError<DomainError>(out IEnumerable<DomainError> domainErrors))
        {
            var targetError = domainErrors.First(); //return only first error as API response
            return targetError.MapToProblemDetails();
        }
        else if (failedDomainResult.IsFailed)
        {
            return Results.Problem(ApiErrors.UnexpectedError);
        }

        throw new ArgumentException("Only unsuccessfull results can be handled");
    }
}
