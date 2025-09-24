using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Libba.HubTo.Arcavis.WebApi.ActionFilters;

public class ValidateModelActionFilter : IActionFilter
{
    private readonly ILogger<ValidateModelActionFilter> _logger;

    public ValidateModelActionFilter(ILogger<ValidateModelActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            _logger.LogWarning("Model Valid Değil");
        }
        _logger.LogInformation("Model Valid.");
    }
}