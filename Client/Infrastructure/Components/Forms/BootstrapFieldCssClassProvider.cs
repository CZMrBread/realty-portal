using Microsoft.AspNetCore.Components.Forms;

namespace Client.Infrastructure.Components.Forms;

/// <summary>
/// Emits Bootstrap <c>is-invalid</c> for any field with messages and <c>is-valid</c> only for modified fields.
/// </summary>
public sealed class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    /// <summary>Shared stateless instance.</summary>
    public static readonly BootstrapFieldCssClassProvider Instance = new();

    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        if (editContext.GetValidationMessages(fieldIdentifier).Any())
        {
            return "is-invalid";
        }

        return editContext.IsModified(fieldIdentifier) ? "is-valid" : string.Empty;
    }
}
