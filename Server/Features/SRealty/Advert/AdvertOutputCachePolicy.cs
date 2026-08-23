using Microsoft.AspNetCore.OutputCaching;

namespace Server.Features.SRealty.Advert;

/// <summary>
/// Tags the cached response with the advert it was built from, so a write can evict exactly that entry.
/// Cacheability itself is left to the default policy, which already refuses anything but an anonymous
/// GET returning 200.
/// </summary>
public sealed class AdvertOutputCachePolicy : IOutputCachePolicy
{
    /// <summary>Tag every cached response for one advert carries, and the one a write evicts by.</summary>
    public static string Tag(Guid advertId) => $"advert:{advertId}";

    /// <inheritdoc />
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        if (context.HttpContext.Request.RouteValues.TryGetValue("advertId", out var routeValue)
            && Guid.TryParse(routeValue?.ToString(), out var advertId))
        {
            context.Tags.Add(Tag(advertId));
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    /// <inheritdoc />
    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;
}
