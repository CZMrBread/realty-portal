using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.RealtyAgency.Entity;

/// <summary>A real estate agency. It owns the agents working under it and every advert they publish.</summary>
public sealed class RealtyAgencyEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    /// <summary>Search key derived from <see cref="Name"/>, kept by the service so the trigram index has a real column to sit on.</summary>
    [MaxLength(200)]
    public string SearchName { get; set; } = string.Empty;

    /// <summary>Company registration number the agency is entered in the business register under.</summary>
    [MaxLength(32)]
    public required string RegistrationNumber { get; set; }

    [EmailAddress]
    [MaxLength(256)]
    public required string Email { get; set; }

    /// <summary>Agents working under the agency.</summary>
    [JsonIgnore]
    public List<RealtyAgentEntity> Agents { get; set; } = [];
}
