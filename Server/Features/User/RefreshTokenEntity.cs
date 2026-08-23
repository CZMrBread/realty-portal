using System.ComponentModel.DataAnnotations;
using Server.Infrastructure.Database;

namespace Server.Features.User;

/// <summary>
/// One issued refresh token. The token itself is never stored, only its hash, so that a leaked database does not
/// hand out sessions. A token is used once: refreshing revokes it and records which token replaced it, which is
/// what makes a replayed token detectable.
/// </summary>
public class RefreshTokenEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    /// <summary>Hash of the token. The token itself is only ever seen by the client it was issued to.</summary>
    [Required] public required string TokenHash { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
    /// <summary>When the token was withdrawn, or null while it is still good.</summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>Whether the token has outlived its expiry. Says nothing about whether it was revoked.</summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [Required] public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    /// <summary>Hash of the token issued in place of this one when it was last refreshed.</summary>
    public string? ReplacedByHash { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}