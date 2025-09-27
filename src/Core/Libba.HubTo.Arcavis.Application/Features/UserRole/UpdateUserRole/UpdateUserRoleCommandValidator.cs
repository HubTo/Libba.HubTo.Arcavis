using FluentValidation;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UpdateUserRoleCommandValidator(
        IUserRepository userRepository, 
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;

        InitializeRules();
    }
    #endregion

    public void InitializeRules()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ID of the relationship cannot be empty.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID cannot be empty.");

        RuleFor(x => x.UserId)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("User ID cannot be empty.")
                    .MustAsync(UserMustExist).WithMessage("The specified User does not exist.");

        RuleFor(x => x.RoleId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role ID cannot be empty.")
            .MustAsync(RoleMustExist).WithMessage("The specified Role does not exist.");

    }

    private async Task<bool> UserMustExist(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.ExistsAsync(u => u.Id == userId, cancellationToken);
    }

    private async Task<bool> RoleMustExist(Guid roleId, CancellationToken cancellationToken)
    {
        return await _roleRepository.ExistsAsync(r => r.Id == roleId, cancellationToken);
    }
}
