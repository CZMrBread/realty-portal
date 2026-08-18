namespace Shared.SRealty.Enums.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class SRealtyEnumsAttribute : Attribute
{
    public string DisplayNameCz { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public string DescriptionCz { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}