using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class UserRoleConfiguration : BaseConfiguration<UserRoleEntity>
{
    public override void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("USER_ROLE");

        builder.HasOne(ur => ur.User).WithMany(u => u.UserRoleEntities).HasForeignKey("USER_ID").IsRequired();
        builder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey("ROLE_ID").IsRequired();
    }
}
