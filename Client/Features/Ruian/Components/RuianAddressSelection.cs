using Shared.Ruian;
using Shared.Ruian.GetAddressPoints;
using Shared.SRealty.Advert.Enums;

namespace Client.Features.Ruian.Components;

/// <summary>What the address picker has chosen so far; a deeper level implies the ones above it.</summary>
public sealed record RuianAddressSelection(
    RuianPlaceDto? Region,
    RuianPlaceDto? District,
    RuianPlaceDto? Municipality,
    RuianPlaceDto? Part,
    RuianPlaceDto? Street,
    RuianAddressPointDto? AddressPoint)
{
    /// <summary>RUIAN code of the deepest chosen level, or null when nothing is chosen.</summary>
    public int? Code => AddressPoint?.Code ?? Street?.Code ?? Municipality?.Code ?? District?.Code;

    /// <summary>Level of <see cref="Code"/>, or null when nothing is chosen.</summary>
    public RuianLevelEnum? Level => AddressPoint is not null ? RuianLevelEnum.Address
        : Street is not null ? RuianLevelEnum.Street
        : Municipality is not null ? RuianLevelEnum.Municipality
        : District is not null ? RuianLevelEnum.District
        : null;
}
