using FluentValidation;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;

public class UpdateRoleEndpointCommandValidator : AbstractValidator<UpdateRoleEndpointCommand>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IEndpointRepository _endpointRepository;

    public UpdateRoleEndpointCommandValidator(
        IRoleRepository roleRepository, 
        IEndpointRepository endpointRepository)
    {
        _roleRepository = roleRepository;
        _endpointRepository = endpointRepository;

        InitializeRules();
    }
    #endregion

    private void InitializeRules()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The ID of the relationship cannot be empty.");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID cannot be empty.");

        RuleFor(x => x.EndpointId)
            .NotEmpty().WithMessage("Endpoint ID cannot be empty.");

        RuleFor(x => x.RoleId)
            .MustAsync(RoleMustExist)
            .WithMessage("The specified Role does not exist.")
            .When(x => x.RoleId != Guid.Empty);

        RuleFor(x => x.EndpointId)
            .MustAsync(EndpointMustExist)
            .WithMessage("The specified Endpoint does not exist.")
            .When(x => x.EndpointId != Guid.Empty);
    }

    private async Task<bool> RoleMustExist(Guid roleId, CancellationToken cancellationToken)
    {
        return await _roleRepository.ExistsAsync(roleId, cancellationToken);
    }

    private async Task<bool> EndpointMustExist(Guid endpointId, CancellationToken cancellationToken)
    {
        return await _endpointRepository.ExistsAsync(endpointId, cancellationToken);
    }
}
