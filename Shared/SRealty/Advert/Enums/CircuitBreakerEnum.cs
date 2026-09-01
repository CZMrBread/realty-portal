using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Rated current of the main circuit breaker.</summary>
public enum CircuitBreakerEnum
{
    [LocalizedDisplayName(DisplayNameCz = "16A", DisplayNameEn = "16A")]
    A16 = 1,

    [LocalizedDisplayName(DisplayNameCz = "20A", DisplayNameEn = "20A")]
    A20 = 2,

    [LocalizedDisplayName(DisplayNameCz = "25A", DisplayNameEn = "25A")]
    A25 = 3,

    [LocalizedDisplayName(DisplayNameCz = "32A", DisplayNameEn = "32A")]
    A32 = 4,

    [LocalizedDisplayName(DisplayNameCz = "40A", DisplayNameEn = "40A")]
    A40 = 5,

    [LocalizedDisplayName(DisplayNameCz = "50A", DisplayNameEn = "50A")]
    A50 = 6,

    [LocalizedDisplayName(DisplayNameCz = "63A", DisplayNameEn = "63A")]
    A63 = 7
}