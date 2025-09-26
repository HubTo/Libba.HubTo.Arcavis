using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;

public class CreateRoleEndpointCommandValidator : AbstractValidator<CreateRoleEndpointCommand>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IRoleEndpointRepository _roleEndpointRepository;

    public CreateRoleEndpointCommandValidator(
        IRoleRepository roleRepository, 
        IEndpointRepository endpointRepository, 
        IRoleEndpointRepository roleEndpointRepository)
    {
        _roleRepository = roleRepository;
        _endpointRepository = endpointRepository;
        _roleEndpointRepository = roleEndpointRepository;

        InitializeRules();
    }
    #endregion

    private void InitializeRules()
    {
        RuleFor(x => x.RoleId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role ID cannot be empty.")
            .MustAsync(RoleMustExist).WithMessage("The specified Role does not exist.");

        RuleFor(x => x.EndpointId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Endpoint ID cannot be empty.")
            .MustAsync(EndpointMustExist).WithMessage("The specified Endpoint does not exist.");

        RuleFor(x => x)
            .MustAsync(RelationMustNotExist)
            .WithMessage("This relationship between the Role and Endpoint already exists.")
            .When(x => x.RoleId != Guid.Empty && x.EndpointId != Guid.Empty);
    }

    private async Task<bool> RoleMustExist(Guid roleId, CancellationToken cancellationToken)
    {
        return await _roleRepository.ExistsAsync(roleId, cancellationToken);
    }

    private async Task<bool> EndpointMustExist(Guid endpointId, CancellationToken cancellationToken)
    {
        return await _endpointRepository.ExistsAsync(endpointId, cancellationToken);
    }

    private async Task<bool> RelationMustNotExist(CreateRoleEndpointCommand command, CancellationToken cancellationToken)
    {
        return !await _roleEndpointRepository.DoesRelationExistAsync(command.RoleId, command.EndpointId, cancellationToken);
    }
}
