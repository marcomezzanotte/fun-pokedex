using Microsoft.AspNetCore.Http;

namespace FunPokedex.Core.Errors;

public static class AspNetCoreDomainErrorExtensions
{
    public static IResult MapToProblemDetails(this DomainError targetError)
    {
        var statusCode = MapStatusCode(targetError);
        return Results.Problem(statusCode: statusCode, title: targetError.Message, type: targetError.Code.Value, extensions: targetError.Metadata);
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
