using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.DeleteRoleEndpoint;

public class DeleteRoleEndpointCommandHandler : ICommandHandler<DeleteRoleEndpointCommand, bool>
{
    #region Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepository;


    public DeleteRoleEndpointCommandHandler(
        IRoleEndpointRepository roleEndpointRepository)
    {
        _roleEndpointRepository = roleEndpointRepository;
    }
    #endregion

    public async Task<bool> Handle(DeleteRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = await _roleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            return false;

        _roleEndpointRepository.Delete(dal);

        return true;
    }
}