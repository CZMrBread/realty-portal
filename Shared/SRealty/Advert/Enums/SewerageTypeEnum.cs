using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>How waste water leaves the property.</summary>
public enum SewerageTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Veřejná kanalizace", DisplayNameEn = "Public sewer")]
    PublicSewer = 1,

    [LocalizedDisplayName(DisplayNameCz = "Čistička odpadních vod pro celý objekt", DisplayNameEn = "Object treatment plant")]
    ObjectTreatmentPlant = 2,

    [LocalizedDisplayName(DisplayNameCz = "Septik", DisplayNameEn = "Septic tank")]
    SepticTank = 3,

    [LocalizedDisplayName(DisplayNameCz = "Jímka", DisplayNameEn = "Cesspool")]
    Cesspool = 4,

    [LocalizedDisplayName(DisplayNameCz = "Trativod", DisplayNameEn = "Drainage")]
    Drainage = 5
}