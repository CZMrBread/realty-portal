namespace Shared.Shared.Extensions;

public static class PageResultExtensions
{
    public static async Task<PagedResult<TSource>> ToPagedResultAsync<TSource>(this IQueryable<TSource> query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = await query.ToAsyncEnumerable().ToListAsync(cancellationToken);
        var totalCount = await query.ToAsyncEnumerable().CountAsync(cancellationToken);
        return new PagedResult<TSource>(items, page, pageSize, totalCount);
    }
}
