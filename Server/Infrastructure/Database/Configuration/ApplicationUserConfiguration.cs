using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgent;
using Server.Features.User;

namespace Server.Infrastructure.Database.Configuration;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.HasDiscriminator<string>("UserType")
            .HasValue<ApplicationUser>(ApplicationRole.User)
            .HasValue<RealtyAgentEntity>(ApplicationRole.RealtyAgent)
            .HasValue<RealtyAgencyAdminEntity>(ApplicationRole.RealtyAgencyAdmin);
    }
}