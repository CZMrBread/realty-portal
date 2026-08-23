using System.Net;

namespace Shared.Shared;

/// <summary>
/// One way a request can be refused, named once and used everywhere. Binding the three parts together is the
/// point: a refusal always answers with the same status and the same wording, whichever endpoint raises it.
/// <para>
/// The <see cref="Code"/> is what a caller branches on and what the client looks the reader's own wording up
/// by, so it is part of the API and may not change. <see cref="Detail"/> is the English fallback, shown only
/// where the client has no wording of its own. Where a refusal has to say something particular, take a copy:
/// <c>UserErrors.RegistrationFailed with { Detail = ... }</c>.
/// </para>
/// </summary>
/// <param name="Code">Stable identifier of the refusal, such as <c>user.email_taken</c>.</param>
/// <param name="StatusCode">HTTP status the refusal is answered with.</param>
/// <param name="Detail">English wording, for a caller reading the response by hand.</param>
public sealed record ApiError(string Code, HttpStatusCode StatusCode, string Detail);
