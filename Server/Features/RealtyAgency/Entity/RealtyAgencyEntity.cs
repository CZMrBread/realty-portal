using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;
using Shared.Shared.Extensions;

namespace Server.Features.RealtyAgency.Entity;

/// <summary>A real estate agency. It owns the agents working under it and every advert they publish.</summary>
public sealed class RealtyAgencyEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    [NotMapped]
    public string SearchName { get => Name.ToSearchKey(); }

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
