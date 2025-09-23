using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class TokenConfiguration : BaseConfiguration<TokenEntity>
{
    public override void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("TOKEN");

        builder.Property(e => e.RefreshToken).HasColumnName("REFRESH_TOKEN").IsRequired();
        builder.Property(e => e.ValidDate).HasColumnName("VALID_DATE").IsRequired();

        builder.HasOne(e => e.Users).WithMany(u => u.TokenEntities).HasForeignKey("USER_ID").IsRequired();
    }
}
