using FluentValidation;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Libba.HubTo.Arcavis.Application.Extensions;

public static class Registration
{
    public static IServiceCollection AddArcavisMapper(this IServiceCollection services)
    {
        var assm = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assm);
        services.AddValidatorsFromAssembly(assm);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assm));
        services.AddScoped<IArcavisMapper, ArcavisMapper>();

        return services;
    }
}
