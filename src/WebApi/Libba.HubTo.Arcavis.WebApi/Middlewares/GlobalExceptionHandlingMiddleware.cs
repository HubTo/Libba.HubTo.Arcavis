using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Libba.HubTo.Arcavis.WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    #region Dependencies
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    #endregion

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred: {ErrorMessage}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            var validationResponse = new
            {
                Title = "One or more validation errors occurred.",
                Status = context.Response.StatusCode,
                Errors = errors,
                TraceId = context.TraceIdentifier
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(validationResponse));
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var internalErrorResponse = new
        {
            Title = "An internal server error has occurred.",
            Status = context.Response.StatusCode,
            Detail = "Please try again later or contact support.",
            TraceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(internalErrorResponse));
    }
}
