using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(200).WithMessage("Description cannot be longer than 200 characters.");
    }
}
