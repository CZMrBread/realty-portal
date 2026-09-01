using System.ComponentModel.DataAnnotations;
using Server.Infrastructure.Database;

namespace Server.Features.User;

/// <summary>One issued refresh token, stored as a hash and spent once; refreshing records its replacement.</summary>
public class RefreshTokenEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    /// <summary>Hash of the token; the token itself is never stored.</summary>
    [Required] public required string TokenHash { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
    /// <summary>When the token was revoked, or null while still valid.</summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>Whether the token has passed its expiry; says nothing about revocation.</summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [Required] public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    /// <summary>Hash of the token issued in place of this one on refresh.</summary>
    public string? ReplacedByHash { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}