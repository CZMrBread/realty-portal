using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the agent table, which shares its primary key with the user account, and sets the delete rules for the agency it hangs under.</summary>
public sealed class RealtyAgentConfiguration : IEntityTypeConfiguration<RealtyAgentEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgentEntity> builder)
    {
        // primary key shared with ApplicationUser
        builder.HasKey(a => a.UserId);
        builder.Property(a => a.UserId).ValueGeneratedNever();

        builder.HasOne(a => a.User)
            .WithOne(u => u.RealtyAgent)
            .HasForeignKey<RealtyAgentEntity>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: deleting an agency must not quietly take its agents with it
        builder.HasOne(a => a.RealtyAgency)
            .WithMany(ra => ra.Agents)
            .HasForeignKey(a => a.RealtyAgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // the agency key is unique within one agency, not globally
        builder.HasIndex(a => new { a.RealtyAgencyId, a.RealtyAgentRkId })
            .IsUnique()
            .HasFilter("\"RealtyAgentRkId\" IS NOT NULL");
    }
}
