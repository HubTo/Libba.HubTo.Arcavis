using Microsoft.AspNetCore.Mvc;

namespace Libba.HubTo.Arcavis.WebApi.Extensions;

public static class ApiBehaviorOptionsExtensions
{
    public static IMvcBuilder ConfigureCustomApiBehavior(this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var response = new
                {
                    Title = "An internal server error has occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Please try again later or contact support.",
                    TraceId = context.HttpContext.TraceIdentifier
                };

                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            };
        });
    }
}
