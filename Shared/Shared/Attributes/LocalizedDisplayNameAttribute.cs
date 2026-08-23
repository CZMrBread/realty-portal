namespace Shared.Shared.Attributes;

/// <summary>
/// Labels an enum member with the texts the user interface shows for it. The Czech and English wording is kept
/// next to the member instead of in resource files, and is read through the extension methods in SRealtyEnumsExtension.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class LocalizedDisplayNameAttribute : Attribute
{
    public string DisplayNameCz { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public string DescriptionCz { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    /// <summary>Name of the icon shown with the member, if it has one.</summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>Set to false to keep a member that must still be understood but should no longer be offered.</summary>
    public bool IsActive { get; set; } = true;
}