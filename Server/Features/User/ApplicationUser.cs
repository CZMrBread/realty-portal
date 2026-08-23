using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.User;

/// <summary>A user account. Identity keeps the credentials; the portal adds the timestamps and the link to an agent profile.</summary>
public class ApplicationUser : IdentityUser<Guid>, ITimeStampedEntity
{
    /// <summary>Creates an account with the given user name.</summary>
    public ApplicationUser(string userName)
    {
        UserName = userName;
    }

    public override Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    /// <summary>Agent profile of this account, or null when the user is not an agent.</summary>
    [JsonIgnore]
    public RealtyAgentEntity? RealtyAgent { get; set; }
    
    /// <summary>Refresh tokens ever issued to this account, including the spent and revoked ones.</summary>
    [JsonIgnore]
    public List<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}