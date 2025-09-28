using System.ComponentModel.DataAnnotations;

namespace Server.Entities.Users;

public class RefreshTokenEntity : ITimeStampedEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required]
    public string Token { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    [Required]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public string? ReplacedByToken { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}