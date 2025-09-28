using Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Extensions;

public static class Registration
{
    public static IServiceCollection AddEfCoreRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ArcavisContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            }));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes
                .Where(type => type.Name.EndsWith("Repository")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
}