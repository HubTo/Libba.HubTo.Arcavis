using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Domain.Entities;
using System.Linq.Expressions;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.User.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;

    public CreateUserCommandValidator(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;

        InitializeRules();
    }
    #endregion

    public void InitializeRules()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .Length(7, 20).WithMessage("Phone number must be between 7 and 20 digits.");

        RuleFor(x => x.PhoneCode)
            .NotEmpty().WithMessage("Phone code cannot be empty.")
            .Length(2, 10).WithMessage("Phone code must be between 2 and 10 digits.");

        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MustAsync(BeUniquePhoneNumber)
            .WithMessage("This phone number is already registered.")
            .When(command =>
                !string.IsNullOrWhiteSpace(command.PhoneCode) &&
                !string.IsNullOrWhiteSpace(command.PhoneNumber));
    }

    private async Task<bool> BeUniquePhoneNumber(CreateUserCommand command, CancellationToken cancellationToken)
    {
        Expression<Func<UserEntity, bool>> predicate = user =>
            user.PhoneCode == command.PhoneCode &&
            user.PhoneNumber == command.PhoneNumber;

        return !await _userRepository.ExistsAsync(predicate, cancellationToken);
    }
}
