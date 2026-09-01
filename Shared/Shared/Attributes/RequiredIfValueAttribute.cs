using System.ComponentModel.DataAnnotations;

namespace Shared.Shared.Attributes;

/// <summary>Makes a property required only when another property of the same object holds a listed value.</summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfValueAttribute : ValidationAttribute
{
    /// <summary>Name of the property that decides whether this one is required.</summary>
    public string PropertyName { get; }

    /// <summary>Values of that property which make this one required.</summary>
    public object?[] Values { get; }

    /// <summary>Creates the attribute.</summary>
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