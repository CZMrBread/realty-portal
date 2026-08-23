using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>State of the building, from a finished new build to a property awaiting demolition.</summary>
public enum BuildingConditionEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Velmi dobrý", DisplayNameEn = "Very good")]
    VeryGood = 1,

    [LocalizedDisplayName(DisplayNameCz = "Dobrý", DisplayNameEn = "Good")]
    Good = 2,

    [LocalizedDisplayName(DisplayNameCz = "Špatný", DisplayNameEn = "Poor")]
    Poor = 3,

    [LocalizedDisplayName(DisplayNameCz = "Ve výstavbě", DisplayNameEn = "Under construction")]
    UnderConstruction = 4,

    [LocalizedDisplayName(DisplayNameCz = "Projekt", DisplayNameEn = "Project")]
    Project = 5,

    [LocalizedDisplayName(DisplayNameCz = "Novostavba", DisplayNameEn = "New building")]
    NewBuilding = 6,

    [LocalizedDisplayName(DisplayNameCz = "K demolici", DisplayNameEn = "To demolish")]
    ToDemolish = 7,

    [LocalizedDisplayName(DisplayNameCz = "Před rekonstrukcí", DisplayNameEn = "Before renovation")]
    BeforeRenovation = 8,

    [LocalizedDisplayName(DisplayNameCz = "Po rekonstrukci", DisplayNameEn = "After renovation")]
    AfterRenovation = 9,

    [LocalizedDisplayName(DisplayNameCz = "V rekonstrukci", DisplayNameEn = "In renovation")]
    InRenovation = 10
}