using Shared.Shared;

namespace Server.Infrastructure.Http;

/// <summary>Converts an <see cref="ApiError"/> into the API's error response.</summary>
public static class ApiErrorResults
{
    /// <summary>The error as an RFC 9457 problem document with the code in the <c>errorCode</c> extension.</summary>
    public static IResult ToResult(this ApiError error)
        => TypedResults.Problem(
            detail: error.Detail,
            statusCode: (int)error.StatusCode,
            extensions: new Dictionary<string, object?> { ["errorCode"] = error.Code });
}
