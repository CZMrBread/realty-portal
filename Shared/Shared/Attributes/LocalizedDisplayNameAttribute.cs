namespace Shared.Shared.Attributes;

/// <summary>Czech and English UI texts of an enum member, read through SRealtyEnumsExtension.</summary>
[AttributeUsage(AttributeTargets.Field)]
public class LocalizedDisplayNameAttribute : Attribute
{
    public string DisplayNameCz { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public string DescriptionCz { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    /// <summary>Name of the icon shown with the member, if it has one.</summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>False keeps the member recognized but no longer offered.</summary>
    public bool IsActive { get; set; } = true;
}