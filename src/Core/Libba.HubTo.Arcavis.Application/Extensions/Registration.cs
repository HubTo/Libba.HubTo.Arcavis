using FluentValidation;
using Libba.HubTo.Arcavis.Application.Behaviors;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Libba.HubTo.Arcavis.Application.Extensions;

public static class Registration
{
    public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
    {
        var assm = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assm);
        services.AddValidatorsFromAssembly(assm);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assm);

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        services.AddScoped<IArcavisMapper, ArcavisMapper>();
        services.AddScoped<IRequestContext, RequestContext>();
        services.AddScoped<IArcavisCQRS, ArcavisCQRS>();

        return services;
    }
}
