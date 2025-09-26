using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;

public class UpdateEndpointCommandValidator : AbstractValidator<UpdateEndpointCommand>
{
    public UpdateEndpointCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty.");

        RuleFor(x => x.ModuleName)
            .NotEmpty().WithMessage("Module name cannot be empty.")
            .MaximumLength(50).WithMessage("Module name cannot be longer than 50 characters.");

        RuleFor(x => x.ControllerName)
            .NotEmpty().WithMessage("Controller name cannot be empty.")
            .MaximumLength(100).WithMessage("Module name cannot be longer than 50 characters.");

        RuleFor(x => x.ActionName)
            .NotEmpty().WithMessage("Action name cannot be empty.")
            .MaximumLength(100).WithMessage("Action name cannot be longer than 50 characters.");

        RuleFor(x => x.Namespace)
            .NotEmpty().WithMessage("Namespace cannot be empty.")
            .MaximumLength(200).WithMessage("Namespace cannot be longer than 50 characters.");

        RuleFor(x => x.HttpVerb)
            .IsInEnum().WithMessage("A valid HTTP verb must be specified.");
    }
}
