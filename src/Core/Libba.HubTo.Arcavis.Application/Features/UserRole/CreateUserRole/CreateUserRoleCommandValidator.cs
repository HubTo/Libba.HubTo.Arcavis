using FluentValidation;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public CreateUserRoleCommandValidator(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;

        InitializeRules();
    }
    #endregion

    private void InitializeRules()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("User ID cannot be empty.")
            .MustAsync(UserMustExist).WithMessage("The specified User does not exist.");

        RuleFor(x => x.RoleId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role ID cannot be empty.")
            .MustAsync(RoleMustExist).WithMessage("The specified Role does not exist.");

        RuleFor(x => x)
            .MustAsync(RelationMustNotExist)
            .WithMessage("This relationship between the User and Role already exists.")
            .When(x => x.UserId != Guid.Empty && x.RoleId != Guid.Empty);
    }

    private async Task<bool> UserMustExist(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.ExistsAsync(u => u.Id == userId, cancellationToken);
    }

    private async Task<bool> RoleMustExist(Guid roleId, CancellationToken cancellationToken)
    {
        return await _roleRepository.ExistsAsync(r => r.Id == roleId, cancellationToken);
    }

    private async Task<bool> RelationMustNotExist(CreateUserRoleCommand command, CancellationToken cancellationToken)
    {
        return !await _userRoleRepository.DoesRelationExistAsync(command.UserId, command.RoleId, cancellationToken);
    }
}
