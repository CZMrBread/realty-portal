using System.ComponentModel.DataAnnotations;

namespace Shared.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfValueAttribute : ValidationAttribute
{
    private string PropertyName { get; }
    private object[] Values { get; }

    public RequiredIfValueAttribute(string propertyName, params object[] values)
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