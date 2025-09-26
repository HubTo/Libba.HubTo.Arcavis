using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;

public class CreateRoleEndpointCommandHandler : ICommandHandler<CreateRoleEndpointCommand, Guid>
{
    #region Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;

    public CreateRoleEndpointCommandHandler(
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<RoleEndpointEntity>(request);

        await _roleEndpointRepository.AddAsync(dal, cancellationToken);

        return dal.Id;
    }
}