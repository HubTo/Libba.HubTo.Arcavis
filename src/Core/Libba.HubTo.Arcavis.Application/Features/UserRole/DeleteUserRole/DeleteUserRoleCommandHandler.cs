using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.DeleteUserRole;

public class DeleteUserRoleCommandHandler : ICommandHandler<DeleteUserRoleCommand, bool>
{
    #region Dependencies
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public DeleteUserRoleCommandHandler(
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        _userRoleRepository.Delete(dal);

        return true;
    }
}