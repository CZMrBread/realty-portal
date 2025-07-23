using Microsoft.AspNetCore.Identity;

namespace Server.Entities;

public class ApplicationRole: IdentityRole<Guid>, ITimeStampedEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string User = "User";
}