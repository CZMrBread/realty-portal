using System.ComponentModel.DataAnnotations;

namespace Shared.Shared.Attributes;

/// <summary>
/// Makes a property required only when another property on the same object holds one of the listed values,
/// which is how the advert model expresses fields that matter for some property categories and not for others.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfValueAttribute : ValidationAttribute
{
    private string PropertyName { get; }
    private object?[] Values { get; }

    /// <summary>Creates the attribute.</summary>
    /// <param name="propertyName">Name of the property that decides whether this one is required.</param>
    /// <param name="values">Values of that property which make this one required.</param>
    public RequiredIfValueAttribute(string propertyName, params object?[] values)
    {
        PropertyName = propertyName;
        Values = values;
        ErrorMessage = ErrorMessage ?? $"{{0}} is required when {propertyName} is one of: {string.Join(", ", values)}.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(PropertyName);

        if (property == null)
        {
            return new ValidationResult($"Unknown property: {PropertyName}");
        }

        var propertyValue = property.GetValue(validationContext.ObjectInstance);

        bool matchesCondition = Values.Any(v =>
            (v == null && propertyValue == null) ||
            (v != null && v.Equals(propertyValue)));

        if (!matchesCondition)
        {
            return ValidationResult.Success;
        }

        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
        {
            return new ValidationResult(
                FormatErrorMessage(validationContext.DisplayName),
                [validationContext.MemberName ?? string.Empty]);
        }

        return ValidationResult.Success;
    }
}