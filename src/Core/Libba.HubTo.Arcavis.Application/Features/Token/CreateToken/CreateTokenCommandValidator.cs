using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    #region Dependencies
    public CreateTokenCommandValidator()
    {
        InitializeRules();
    }
    #endregion

    public void InitializeRules()
    {

    }
}
