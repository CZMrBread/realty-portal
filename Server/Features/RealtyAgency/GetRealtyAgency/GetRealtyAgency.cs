using Server.Features.RealtyAgency.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;

namespace Server.Features.RealtyAgency.GetRealtyAgency;

/// <summary>Returns a single agency.</summary>
public static class GetRealtyAgency
{
    /// <summary>Registers the read routes: by identifier and by registration number.</summary>
    public static void MapGetRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{agencyId:guid}", GetRealtyAgencyByIdAsync)
            .WithName(nameof(GetRealtyAgencyByIdAsync));

        group.MapGet("/registration/{registrationNumber}", GetRealtyAgencyByRegistrationNumberAsync)
            .WithName(nameof(GetRealtyAgencyByRegistrationNumberAsync));
    }

    /// <summary>Reads the agency with <paramref name="agencyId"/>; public.</summary>
    internal static async Task<IResult> GetRealtyAgencyByIdAsync(
        Guid agencyId,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
    {
        return Respond(await realtyAgencyService.FindAgencyByIdAsync(agencyId, cancellationToken));
    }

    /// <summary>Reads the agency with <paramref name="registrationNumber"/>; public.</summary>
    internal static async Task<IResult> GetRealtyAgencyByRegistrationNumberAsync(
        string registrationNumber,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
    {
        return Respond(
            await realtyAgencyService.FindAgencyByRegistrationNumberAsync(registrationNumber, cancellationToken));
    }

    /// <summary>Maps the resolved agency to a response, or 404 when null.</summary>
    private static IResult Respond(RealtyAgencyEntity? agency)
        => agency is null
            ? AgencyErrors.NotFound.ToResult()
            : TypedResults.Ok(agency.ToGetResponse());
}
