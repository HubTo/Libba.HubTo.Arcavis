using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var requestJson = JsonSerializer.Serialize(request);

        _logger.LogInformation("Handling request: {RequestName}. Request body: {RequestJson}", requestName, requestJson);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();

            stopwatch.Stop();

            _logger.LogInformation("Request {RequestName} handled successfully in {ElapsedMilliseconds}ms.", requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "An error occurred while handling {RequestName} in {ElapsedMilliseconds}ms. Request body: {RequestJson}",
                requestName, stopwatch.ElapsedMilliseconds, requestJson);

            throw;
        }
    }
}
