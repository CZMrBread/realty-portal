using Shared.Shared.Attributes;

namespace Shared.SRealty.Photo;

/// <summary>Kind of image: ordinary photo, 360-degree panorama or floor plan.</summary>
public enum PhotoKindEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Fotografie inzerátu", DisplayNameEn = "Photo")]
    Photo = 1,
    [LocalizedDisplayName(DisplayNameCz = "Sférická fotografie", DisplayNameEn = "360° Photo")]
    Photo360 = 2,
    [LocalizedDisplayName(DisplayNameCz = "Půdorys", DisplayNameEn = "Floor Plan")]
    FloorPlan = 3,
}