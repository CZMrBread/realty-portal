using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Infrastructure.Components.Forms;

/// <summary>Base for wrapped form controls: bound value, label, id and validation messages.</summary>
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

    /// <summary>Hint shown inside the empty control; a blank one is supplied when not given.</summary>
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>Note under the control, replaced by the error while invalid.</summary>
    [Parameter] public string? Help { get; set; }

    /// <summary>Id of the control; generated when not given.</summary>
    [Parameter] public string? Id { get; set; }

    /// <summary>Classes of the wrapping element; replaces the default.</summary>
    [Parameter] public string WrapperClass { get; set; } = "mb-3";

    /// <summary>Unmatched attributes, passed on to the control itself.</summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected string FieldId => Id ?? generatedId;

    /// <summary>
    /// The bound field; controls built on a plain input must pass it to
    /// <see cref="Microsoft.AspNetCore.Components.Forms.EditContext.NotifyFieldChanged"/> themselves.
    /// </summary>
    protected FieldIdentifier Field => fieldIdentifier;

    protected string PlaceholderText => string.IsNullOrEmpty(Placeholder) ? " " : Placeholder;

    /// <summary>Required Bootstrap class of the control; defaults to form-control.</summary>
    protected virtual string ControlBaseClass => "form-control";

    /// <summary>Control classes: <see cref="ControlBaseClass"/> merged with the caller's class attribute.</summary>
    protected string ControlClass { get; private set; } = string.Empty;

    /// <summary>Caller attributes without class, which is merged into <see cref="ControlClass"/>.</summary>
    protected IReadOnlyDictionary<string, object>? ControlAttributes { get; private set; }

    /// <summary>Validation messages of this field; empty when not bound to an edit context.</summary>
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
