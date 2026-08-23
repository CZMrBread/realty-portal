using System.Net;

namespace Client.Tests.TestDoubles;

/// <summary>
/// Innermost handler that answers from a script instead of the network, and keeps every request it was given
/// so that a test can check what was sent and how often.
/// </summary>
public sealed class StubHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
{
    private readonly List<HttpRequestMessage> requests = [];

    public IReadOnlyList<HttpRequestMessage> Requests => requests;

    public int CountTo(string pathSuffix)
        => requests.Count(r => r.RequestUri!.AbsolutePath.EndsWith(pathSuffix, StringComparison.Ordinal));

    /// <summary>Answers everything with the given status and body.</summary>
    public static StubHttpHandler Always(HttpStatusCode status, string body = "")
        => new(_ => new HttpResponseMessage(status) { Content = new StringContent(body) });

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        requests.Add(request);

        // the real network is never instant, and a same-tick answer would hide races the lock is there to stop
        await Task.Yield();
        return respond(request);
    }
}
