using Libba.HubTo.Arcavis.Application.Services.User.Commands;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Services.User.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        
    }
}
