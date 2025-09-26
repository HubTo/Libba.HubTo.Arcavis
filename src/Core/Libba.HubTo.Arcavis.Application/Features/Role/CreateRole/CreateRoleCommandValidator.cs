using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    #region Dependencies
    public CreateRoleCommandValidator()
    {
        InitializeRules();
    }
    #endregion

    public void InitializeRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(200).WithMessage("Description cannot be longer than 200 characters.");
    }
}
