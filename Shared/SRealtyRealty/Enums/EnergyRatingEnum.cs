namespace Shared.SRealtyRealty.Enums;

public enum EnergyRatingEnum
{
    [SRealtyEnums(DisplayNameCz = "A", DisplayNameEn = "A",
        DescriptionCz = "Velmi úsporná - nejlepší energetická třída",
        DescriptionEn = "Very efficient - best energy class", Icon = "bi-battery-full")]
    A = 1,

    [SRealtyEnums(DisplayNameCz = "B", DisplayNameEn = "B", DescriptionCz = "Úsporná - velmi dobrá energetická třída",
        DescriptionEn = "Efficient - very good energy class", Icon = "bi-battery-half")]
    B = 2,

    [SRealtyEnums(DisplayNameCz = "C", DisplayNameEn = "C", DescriptionCz = "Vyhovující - dobrá energetická třída",
        DescriptionEn = "Adequate - good energy class", Icon = "bi-battery")]
    C = 3,

    [SRealtyEnums(DisplayNameCz = "D", DisplayNameEn = "D", DescriptionCz = "Méně úsporná - průměrná energetická třída",
        DescriptionEn = "Less efficient - average energy class", Icon = "bi-speedometer")]
    D = 4,

    [SRealtyEnums(DisplayNameCz = "E", DisplayNameEn = "E", DescriptionCz = "Nehospodárná - horší energetická třída",
        DescriptionEn = "Inefficient - worse energy class", Icon = "bi-exclamation-triangle")]
    E = 5,

    [SRealtyEnums(DisplayNameCz = "F", DisplayNameEn = "F",
        DescriptionCz = "Velmi nehospodárná - špatná energetická třída",
        DescriptionEn = "Very inefficient - poor energy class", Icon = "bi-exclamation-diamond")]
    F = 6,

    [SRealtyEnums(DisplayNameCz = "G", DisplayNameEn = "G",
        DescriptionCz = "Mimořádně nehospodárná - nejhorší energetická třída",
        DescriptionEn = "Extremely inefficient - worst energy class", Icon = "bi-x-circle")]
    G = 7
}