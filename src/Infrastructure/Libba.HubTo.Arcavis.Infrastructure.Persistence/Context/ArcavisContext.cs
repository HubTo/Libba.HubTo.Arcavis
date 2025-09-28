using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;

public class ArcavisContext : DbContext
{
    private readonly IRequestContext _requestContext;

    public ArcavisContext(
        DbContextOptions<ArcavisContext> options,
        IRequestContext requestContext) : base(options)
    {
        _requestContext = requestContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArcavisContext).Assembly);
        var entityTypes = typeof(BaseEntity).Assembly
            .GetTypes()
            .Where(t => t.IsClass
                        && !t.IsAbstract
                        && typeof(BaseEntity).IsAssignableFrom(t));
        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType);
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _requestContext.UserId == Guid.Empty ? null : _requestContext.UserId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _requestContext.UserId == Guid.Empty ? null : _requestContext.UserId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _requestContext.UserId == Guid.Empty ? null : _requestContext.UserId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                    if (entry.Entity.CreatedAt.Kind == DateTimeKind.Unspecified)
                    {
                        entry.Entity.CreatedAt = DateTime.SpecifyKind(entry.Entity.CreatedAt, DateTimeKind.Utc);
                    }
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);

    }
}

