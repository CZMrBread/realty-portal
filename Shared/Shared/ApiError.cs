using System.Net;

namespace Shared.Shared;

/// <summary>One kind of request refusal: a stable code, its HTTP status and English fallback wording.</summary>
/// <param name="Code">Stable identifier of the refusal, such as <c>user.email_taken</c>; part of the API.</param>
/// <param name="StatusCode">HTTP status the refusal is answered with.</param>
/// <param name="Detail">English fallback wording, used when the client has none of its own.</param>
public sealed record ApiError(string Code, HttpStatusCode StatusCode, string Detail);
