using Server.Features.RealtyAgency.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;

namespace Server.Features.RealtyAgency.GetRealtyAgency;

/// <summary>Returns a single agency.</summary>
public static class GetRealtyAgency
{
    /// <summary>Name of the by-identifier route, which the create endpoint points its Location header at.</summary>
    public const string ByIdRouteName = "GetRealtyAgencyById";

    /// <summary>Registers the two routes an agency can be read through: by portal identifier, and by the company registration number it is entered under.</summary>
    public static void MapGetRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{agencyId:guid}", GetRealtyAgencyByIdAsync)
            .WithName(ByIdRouteName);

        group.MapGet("/registration/{registrationNumber}", GetRealtyAgencyByRegistrationNumberAsync)
            .WithName("GetRealtyAgencyByRegistrationNumber");
    }

    /// <summary>Reads the agency the portal knows under <paramref name="agencyId"/>. Open to anyone, since an agency is public.</summary>
    private static async Task<IResult> GetRealtyAgencyByIdAsync(
        Guid agencyId,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
    {
        return Respond(await realtyAgencyService.FindAgencyByIdAsync(agencyId, cancellationToken));
    }

    /// <summary>Reads the agency entered under <paramref name="registrationNumber"/>, which names one company across the whole portal.</summary>
    private static async Task<IResult> GetRealtyAgencyByRegistrationNumberAsync(
        string registrationNumber,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
    {
        return Respond(
            await realtyAgencyService.FindAgencyByRegistrationNumberAsync(registrationNumber, cancellationToken));
    }

    /// <summary>Everything both routes do once the agency is in hand.</summary>
    private static IResult Respond(RealtyAgencyEntity? agency)
        => agency is null
            ? AgencyErrors.NotFound.ToResult()
            : TypedResults.Ok(agency.ToDto());
}
