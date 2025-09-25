using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;

public class ArcavisContext : DbContext
{
    public ArcavisContext(DbContextOptions<ArcavisContext> options) : base(options)
    { }

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
}

