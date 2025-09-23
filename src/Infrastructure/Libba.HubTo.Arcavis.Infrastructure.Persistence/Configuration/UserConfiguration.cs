using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public class UserConfiguration : BaseConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("USER");
        
        builder.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(255).IsRequired();
        builder.Property(e => e.PhoneCode).HasColumnName("PHONE_CODE").HasMaxLength(10);
        builder.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(20);
        builder.Property(e => e.PasswordHash).HasColumnName("PASSWORD_HASH").IsRequired();
        builder.Property(e => e.IsAccountActive).HasColumnName("IS_ACCOUNT_ACTIVE").IsRequired().HasDefaultValue(true);
        builder.Property(e => e.IsEmailVerified).HasColumnName("IS_EMAIL_VERIFIED").IsRequired().HasDefaultValue(false);

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.PhoneNumber).IsUnique();
    
        builder.HasMany(e => e.TokenEntities).WithOne(t => t.Users).HasForeignKey("USER_ID");
    }
}
