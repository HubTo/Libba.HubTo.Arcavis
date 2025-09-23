using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class RoleEndpointConfiguration : BaseConfiguration<RoleEndpointEntity>
{
    public override void Configure(EntityTypeBuilder<RoleEndpointEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("ROLE_ENDPOINT");

        builder.HasOne(re => re.Role).WithMany(r => r.RoleEndpoints).HasForeignKey("ROLE_ID").IsRequired();
        builder.HasOne(re => re.Endpoint).WithMany(e => e.RoleEndpoints).HasForeignKey("ENDPOINT_ID").IsRequired();
    }
}
