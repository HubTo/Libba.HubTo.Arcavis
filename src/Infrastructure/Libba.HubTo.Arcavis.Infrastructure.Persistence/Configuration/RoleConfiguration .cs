using Libba.HubTo.Arcavis.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class RoleConfiguration : BaseConfiguration<RoleEntity>
{
    public override void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("ROLE");

        builder.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(200).IsRequired();

        builder.HasMany(e => e.UserRoles).WithOne(ur => ur.Role).HasForeignKey("ROLE_ID");
        builder.HasMany(e => e.RoleEndpoints).WithOne(re => re.Role).HasForeignKey("ROLE_ID");
    }
}
