using Libba.HubTo.Arcavis.Application.Services.Token.Commands;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Validators;

public class DeleteTokenCommandValidator : AbstractValidator<DeleteTokenCommand>
{
    public DeleteTokenCommandValidator()
    {
        
    }
}
