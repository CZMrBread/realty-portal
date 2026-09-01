using Microsoft.AspNetCore.OutputCaching;

namespace Server.Features.SRealty.Advert;

/// <summary>Tags the cached response with the advert it was built from; writes evict by that tag.</summary>
public sealed class AdvertOutputCachePolicy : IOutputCachePolicy
{
    /// <summary>Tag of the cached responses for one advert.</summary>
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
