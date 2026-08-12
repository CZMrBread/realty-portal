using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.RealtyAgency;

namespace Server.Infrastructure.Database.Configuration;

public sealed class RealtyAgencyConfiguration : IEntityTypeConfiguration<RealtyAgencyEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
    }
}