using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.Token.UpdateToken;

public class UpdateTokenCommandValidator : AbstractValidator<UpdateTokenCommand>
{
    #region Dependencies
    public UpdateTokenCommandValidator()
    {
        InitializeRules();
    }
    #endregion

    public void InitializeRules()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ID of the relationship cannot be empty.");
    }
}
