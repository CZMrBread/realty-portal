using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Shared.Validation.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class EnumValueAttribute : ValidationAttribute
{
    private readonly Type _enumType;
    public EnumValueAttribute(Type enumType) => _enumType = enumType;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        switch (value)
        {
            case null:
                return ValidationResult.Success;
            case ICollection collection:
            {
                var invalidItems = new List<object>();
                var index = 0;
                foreach (var item in collection)
                {
                    if (item is not null && !Enum.IsDefined(_enumType, item))
                    {
                        invalidItems.Add($"[{index}]={item}");
                    }
                    index++;
                }

                if (invalidItems.Count > 0)
                {
                    var valid = string.Join(", ",
                        Enum.GetValues(_enumType).Cast<object>().Select(v => $"{v}={Convert.ToInt64(v)}"));
                    var invalidValues = string.Join(", ", invalidItems);
                    return new ValidationResult($"{context.DisplayName} contains invalid values: {invalidValues}. Valid values: [{valid}]",
                        [context.MemberName ?? string.Empty]);
                }
                return ValidationResult.Success;
            }
            default:
                return ValidateSingleEnum(value, context);
        }
    }

    private ValidationResult? ValidateSingleEnum(object? value, ValidationContext context)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (!Enum.IsDefined(_enumType, value))
        {
            var valid = string.Join(", ",
                Enum.GetValues(_enumType).Cast<object>().Select(v => $"{v}={Convert.ToInt64(v)}"));
            return new ValidationResult($"{context.DisplayName} has invalid value '{value}'. Valid values: [{valid}]",
                [context.MemberName ?? string.Empty]);
        }

        return ValidationResult.Success;
    }
}