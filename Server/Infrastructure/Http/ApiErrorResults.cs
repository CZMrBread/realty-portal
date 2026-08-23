using Shared.Shared;

namespace Server.Infrastructure.Http;

/// <summary>Turns a named refusal into the response the API answers with.</summary>
public static class ApiErrorResults
{
    /// <summary>
    /// The refusal as an RFC 9457 problem document. The status says what kind of refusal it is, the
    /// <c>errorCode</c> extension says exactly which one, and the detail is there for a human reading the
    /// response by hand. Callers branch on the code, never on the prose.
    /// </summary>
    public static IResult ToResult(this ApiError error)
        => TypedResults.Problem(
            detail: error.Detail,
            statusCode: (int)error.StatusCode,
            extensions: new Dictionary<string, object?> { ["errorCode"] = error.Code });
}
