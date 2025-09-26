using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, Guid>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"Role with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _roleRepository.Update(dal);

        return dal.Id;
    }
}
