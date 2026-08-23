using Shared.Shared.Attributes;

namespace Shared.SRealty.Photo;

/// <summary>What kind of image the file is: an ordinary photo, a 360-degree panorama or a floor plan.</summary>
public enum PhotoKindEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Fotografie inzerátu", DisplayNameEn = "Photo")]
    Photo = 1,
    [LocalizedDisplayName(DisplayNameCz = "Sférická fotografie", DisplayNameEn = "360° Photo")]
    Photo360 = 2,
    [LocalizedDisplayName(DisplayNameCz = "Půdorys", DisplayNameEn = "Floor Plan")]
    FloorPlan = 3,
}