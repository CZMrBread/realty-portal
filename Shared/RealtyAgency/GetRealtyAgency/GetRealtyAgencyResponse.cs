namespace Shared.RealtyAgency.GetRealtyAgency;

/// <summary>Public view of an agency; the list endpoint pages the same shape.</summary>
public sealed record GetRealtyAgencyResponse
{
    /// <summary>Portal identifier of the agency.</summary>
    public Guid Id { get; set; }

    public string? Name { get; set; }

    /// <summary>Company registration number of the agency.</summary>
    public string? RegistrationNumber { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    /// <summary>Street and house number of the office.</summary>
    public string? Street { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }
}
