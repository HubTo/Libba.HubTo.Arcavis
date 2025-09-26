using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<RoleEntity>(request);

        await _roleRepository.AddAsync(dal, cancellationToken);


        return dal.Id;
    }
}