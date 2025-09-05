using System.ComponentModel.DataAnnotations;
using Shared.SRealtyRealty.Enums;

namespace Shared.Dtos.SRealty.RealtyImport;

public sealed record SrealityAdvertDto : IValidatableObject
{
    
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return Array.Empty<ValidationResult>();
    }
};