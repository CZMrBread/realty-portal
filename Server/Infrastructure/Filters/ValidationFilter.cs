using System.ComponentModel.DataAnnotations;

namespace Server.Infrastructure.Filters;

public class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatableObjects = context.Arguments
            .OfType<IValidatableObject>()
            .ToList();

        if (!validatableObjects.Any())
        {
            return await next(context);
        }

        var allValidationResults = new List<ValidationResult>();

        foreach (var validatableObject in validatableObjects)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(validatableObject);

            Validator.TryValidateObject(validatableObject, validationContext, validationResults, true);

            var customValidationResults = validatableObject.Validate(validationContext);
            validationResults.AddRange(customValidationResults);

            allValidationResults.AddRange(validationResults);
        }

        if (allValidationResults.Any())
        {
            return Results.BadRequest(new
            {
                message = "Validation failed",
                errors = allValidationResults.Select(vr => new
                {
                    propertyName = vr.MemberNames.FirstOrDefault(),
                    errorMessage = vr.ErrorMessage
                })
            });
        }

        return await next(context);
    }
}