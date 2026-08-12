using System.Net;

namespace Client.Tests.TestDoubles;

/// <summary>Fake innermost HttpMessageHandler that records requests and returns a canned response.</summary>
public class RecordingHandler : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }
    public HttpResponseMessage Response { get; set; } = new(HttpStatusCode.OK);

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(Response);
    }
}