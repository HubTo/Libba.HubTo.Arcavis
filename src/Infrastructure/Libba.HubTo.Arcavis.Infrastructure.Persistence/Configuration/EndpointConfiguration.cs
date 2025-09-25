using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class EndpointConfiguration : BaseConfiguration<EndpointEntity>
{
    public override void Configure(EntityTypeBuilder<EndpointEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("ENDPOINT");

        builder.Property(e => e.ModuleName).HasColumnName("MODULE_NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.ControllerName).HasColumnName("CONTROLLER_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.ActionName).HasColumnName("ACTION_NAME").HasMaxLength(100).IsRequired();
        builder.Property(e => e.HttpVerb).HasColumnName("HTTP_VERB").IsRequired();
        builder.Property(e => e.Namespace).HasColumnName("NAMESPACE").HasMaxLength(200).IsRequired();

        builder.HasMany(e => e.RoleEndpoints).WithOne(re => re.Endpoint).HasForeignKey("ENDPOINT_ID");
    }
}

