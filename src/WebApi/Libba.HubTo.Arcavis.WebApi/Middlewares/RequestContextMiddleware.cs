using Libba.HubTo.Arcavis.Application.Interfaces;

namespace Libba.HubTo.Arcavis.WebApi.Middlewares;

public class RequestContextMiddleware
{
    #region Dependencies
    private readonly RequestDelegate _next;

    public RequestContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
    {
        if (context.Request.Headers.TryGetValue("UserId", out var userIdHeader))
        {
            if (Guid.TryParse(userIdHeader, out var userId))
            {
                requestContext.UserId = userId;
            }
        }

        await _next(context);
    }
}
