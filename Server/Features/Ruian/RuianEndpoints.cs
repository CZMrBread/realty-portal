using Server.Features.Ruian.GetAddressPoints;
using Server.Features.Ruian.GetDistricts;
using Server.Features.Ruian.GetMunicipalities;
using Server.Features.Ruian.GetMunicipalityParts;
using Server.Features.Ruian.GetRegions;
using Server.Features.Ruian.GetStreets;
using Server.Features.Ruian.ImportAddressPoints;

namespace Server.Features.Ruian;

/// <summary>Route group of the RUIAN feature.</summary>
public static class RuianEndpoints
{
    /// <summary>Route prefix of the feature.</summary>
    public const string Prefix = "/ruian";

    /// <summary>OpenAPI tag of the feature's routes.</summary>
    public const string Tag = "Ruian";

    /// <summary>Header the address point import route reads its key from; the AppHost sends the same name.</summary>
    public const string ImportKeyHeader = "X-Import-Key";

    /// <summary>Registers the RUIAN routes under <see cref="Prefix"/>.</summary>
    public static void MapRuianEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Prefix).WithTags(Tag);

        group.MapGetRegions();
        group.MapGetDistricts();
        group.MapGetMunicipalities();
        group.MapGetMunicipalityParts();
        group.MapGetStreets();
        group.MapGetAddressPoints();
        group.MapImportAddressPoints();
    }
}
