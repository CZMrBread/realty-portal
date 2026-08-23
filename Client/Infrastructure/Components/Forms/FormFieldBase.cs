using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Infrastructure.Components.Forms;

/// <summary>
/// What every wrapped form control needs: the bound value, a label, an id to tie the two together, and the
/// validation messages belonging to the field. The control itself is left to the derived component, which is
/// what makes one base serve a text box, a number, a date, a select and a checkbox alike.
/// <para>
/// The value is passed straight through to the matching built-in input, so the input keeps doing the parsing
/// and the class provider keeps deciding whether it is green or red. Only the surrounding markup is ours.
/// </para>
/// </summary>
public abstract class FormFieldBase<TValue> : ComponentBase
{
    private readonly string generatedId = $"field-{Guid.CreateVersion7():N}";
    private FieldIdentifier fieldIdentifier;

    [CascadingParameter] protected EditContext? EditContext { get; set; }

    [Parameter] public TValue? Value { get; set; }
    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }

    /// <summary>Text of the floating label.</summary>
    [Parameter] public string? Label { get; set; }

    /// <summary>
    /// Hint shown inside the empty control. Bootstrap needs the attribute to be there at all for a floating
    /// label to work, so a blank one is supplied when no hint is given.
    /// </summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>Note under the control, replaced by the error while the field is invalid.</summary>
    [Parameter] public string? Help { get; set; }

    /// <summary>Id of the control. One is made up when it is not given, so the label always has something to point at.</summary>
    [Parameter] public string? Id { get; set; }

    /// <summary>Classes of the wrapping element, spacing above all. Replaces the default rather than adding to it.</summary>
    [Parameter] public string WrapperClass { get; set; } = "mb-3";

    /// <summary>Anything else is handed to the control itself, so autocomplete, maxlength and the rest still work.</summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected string FieldId => Id ?? generatedId;

    protected string PlaceholderText => string.IsNullOrEmpty(Placeholder) ? " " : Placeholder;

    /// <summary>The Bootstrap class the control cannot do without. Overridden where it is not a form-control.</summary>
    protected virtual string ControlBaseClass => "form-control";

    /// <summary>
    /// Classes for the control: the Bootstrap one it needs, plus whatever the caller asked for.
    /// A caller writing class="..." is adding to the styling, not replacing it, which is why the class is
    /// taken out of the splatted attributes and merged here instead.
    /// </summary>
    protected string ControlClass { get; private set; } = string.Empty;

    /// <summary>Everything the caller passed except the class, which has been merged into <see cref="ControlClass"/>.</summary>
    protected IReadOnlyDictionary<string, object>? ControlAttributes { get; private set; }

    /// <summary>Validation messages of this field, empty when the field is not bound into an edit context.</summary>
    protected IEnumerable<string> ValidationMessages
        => EditContext is null || ValueExpression is null
            ? []
            : EditContext.GetValidationMessages(fieldIdentifier);

    protected override void OnParametersSet()
    {
        if (ValueExpression is not null)
        {
            fieldIdentifier = FieldIdentifier.Create(ValueExpression);
        }

        SplitOutTheClass();
    }

    private void SplitOutTheClass()
    {
        if (AdditionalAttributes is null || !AdditionalAttributes.TryGetValue("class", out var caller))
        {
            ControlClass = ControlBaseClass;
            ControlAttributes = AdditionalAttributes;
            return;
        }

        var extra = caller?.ToString();
        ControlClass = string.IsNullOrWhiteSpace(extra) ? ControlBaseClass : $"{ControlBaseClass} {extra}";
        ControlAttributes = AdditionalAttributes
            .Where(attribute => attribute.Key != "class")
            .ToDictionary(attribute => attribute.Key, attribute => attribute.Value);
    }
}
