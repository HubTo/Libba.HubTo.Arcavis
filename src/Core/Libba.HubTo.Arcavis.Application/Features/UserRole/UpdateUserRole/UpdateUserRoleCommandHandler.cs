using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;

public class UpdateUserRoleCommandHandler : ICommandHandler<UpdateUserRoleCommand, Guid>
{
    #region Dependencies
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateUserRoleCommandHandler(
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"UserRole with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _userRoleRepository.Update(dal);

        return dal.Id;
    }
}
