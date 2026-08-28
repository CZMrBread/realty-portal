namespace Server.Features.RealtyAgency.GetRealtyAgencies;

/// <summary>Returns one page of agencies.</summary>
public static class GetRealtyAgencies
{
    /// <summary>Registers the route the agency list is read through.</summary>
    public static void MapGetRealtyAgencies(this IEndpointRouteBuilder group)
    {
        group.MapGet("", GetRealtyAgenciesAsync)
            .WithName("GetRealtyAgencies");
    }

    /// <summary>
    /// Reads one page of the agencies on the portal, narrowed by <paramref name="name"/> when one is given.
    /// Open to anyone, since an agency is public.
    /// </summary>
    /// <param name="name">Part of the agency name to match, or null to match every agency.</param>
    /// <param name="page">One-based number of the page to read.</param>
    /// <param name="pageSize">Maximum number of agencies the page holds.</param>
    private static Task<IResult> GetRealtyAgenciesAsync(
        string? name,
        int page,
        int pageSize,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
