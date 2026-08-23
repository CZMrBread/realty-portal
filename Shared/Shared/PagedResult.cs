namespace Shared.Shared;

/// <summary>One page of a longer result set, together with what is needed to page through the rest of it.</summary>
/// <typeparam name="T">Type of the items on the page.</typeparam>
/// <param name="Items">Items on this page.</param>
/// <param name="Page">One-based number of this page.</param>
/// <param name="PageSize">Maximum number of items a page holds.</param>
/// <param name="TotalCount">Number of items in the whole result set.</param>
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
{
    /// <summary>Number of pages the whole result set is split into.</summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
