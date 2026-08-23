using Microsoft.AspNetCore.Components.Forms;

namespace Client.Infrastructure.Components.Forms;

/// <summary>
/// Translates what the edit context knows about a field into the classes Bootstrap styles on.
/// Blazor says "modified valid" and "invalid" out of the box, which Bootstrap has no rules for; it wants
/// <c>is-valid</c> and <c>is-invalid</c>.
/// <para>
/// A field only turns green once it has been edited: a form that painted every untouched field green on
/// arrival would be claiming the user had filled in something they had not. A field turns red as soon as it
/// has a message, edited or not, so that submitting an empty form marks everything that is missing.
/// </para>
/// </summary>
public sealed class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    /// <summary>The one instance needed: the provider holds no state of its own.</summary>
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
