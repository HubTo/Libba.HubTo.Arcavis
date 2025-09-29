using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Libba.HubTo.Arcavis.Infrastructure.Logging.Extensions;

public static class Registration
{
    public static IHostBuilder AddSerilogRegistration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly()?.GetName().Name)
                .Enrich.WithExceptionDetails();

            var elasticUri = context.Configuration["Elasticsearch:Uri"];
            if (!string.IsNullOrEmpty(elasticUri))
            {
                configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    NumberOfReplicas = 1,
                    NumberOfShards = 2
                });
            }
        });
        return hostBuilder;
    }
}
