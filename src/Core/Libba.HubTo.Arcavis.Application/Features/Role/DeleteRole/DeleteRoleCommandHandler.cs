using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.DeleteRole;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, bool>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    #endregion

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            return false;

        _roleRepository.Delete(dal);

        return true;
    }
}