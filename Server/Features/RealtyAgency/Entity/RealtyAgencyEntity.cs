using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.RealtyAgency.Entity;

/// <summary>A real estate agency owning its agents and the adverts they publish.</summary>
public sealed class RealtyAgencyEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    /// <summary>Search key derived from <see cref="Name"/>; backs the trigram index.</summary>
    [MaxLength(200)]
    public string SearchName { get; set; } = string.Empty;

    /// <summary>Company registration number of the agency.</summary>
    [MaxLength(32)]
    public required string RegistrationNumber { get; set; }

    [EmailAddress]
    [MaxLength(256)]
    public required string Email { get; set; }

    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    /// <summary>Street and house number of the office.</summary>
    [MaxLength(200)]
    public string? Street { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(16)]
    public string? PostalCode { get; set; }

    /// <summary>Agents of the agency.</summary>
    [JsonIgnore]
    public List<RealtyAgentEntity> Agents { get; set; } = [];
}
