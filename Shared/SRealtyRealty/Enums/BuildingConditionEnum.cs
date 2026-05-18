namespace Shared.SRealtyRealty.Enums;

public enum BuildingConditionEnum
{
    [SRealtyEnums(DisplayNameCz = "Velmi dobrý", DisplayNameEn = "Very good")]
    VeryGood = 1,

    [SRealtyEnums(DisplayNameCz = "Dobrý", DisplayNameEn = "Good")]
    Good = 2,

    [SRealtyEnums(DisplayNameCz = "Špatný", DisplayNameEn = "Poor")]
    Poor = 3,

    [SRealtyEnums(DisplayNameCz = "Ve výstavbě", DisplayNameEn = "Under construction")]
    UnderConstruction = 4,

    [SRealtyEnums(DisplayNameCz = "Projekt", DisplayNameEn = "Project")]
    Project = 5,

    [SRealtyEnums(DisplayNameCz = "Novostavba", DisplayNameEn = "New building")]
    NewBuilding = 6,

    [SRealtyEnums(DisplayNameCz = "K demolici", DisplayNameEn = "To demolish")]
    ToDemolish = 7,

    [SRealtyEnums(DisplayNameCz = "Před rekonstrukcí", DisplayNameEn = "Before renovation")]
    BeforeRenovation = 8,

    [SRealtyEnums(DisplayNameCz = "Po rekonstrukci", DisplayNameEn = "After renovation")]
    AfterRenovation = 9,

    [SRealtyEnums(DisplayNameCz = "V rekonstrukci", DisplayNameEn = "In renovation")]
    InRenovation = 10
}