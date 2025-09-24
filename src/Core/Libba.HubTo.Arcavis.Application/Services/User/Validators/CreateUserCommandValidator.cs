using Libba.HubTo.Arcavis.Application.Services.User.Commands;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Services.User.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        
    }
}
