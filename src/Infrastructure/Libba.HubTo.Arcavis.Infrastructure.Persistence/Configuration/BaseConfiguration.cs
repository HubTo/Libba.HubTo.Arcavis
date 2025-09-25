using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Configuration;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT");
    }
}