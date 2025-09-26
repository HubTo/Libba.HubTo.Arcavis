using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;

public class GetAllRoleEndpointsQueryHandler : IQueryHandler<GetAllRoleEndpointsQuery, IEnumerable<RoleEndpointListItemDto>?>
{
    #region Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllRoleEndpointsQueryHandler(
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleEndpointListItemDto>?> Handle(GetAllRoleEndpointsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _roleEndpointRepository.GetAllAsync(cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<IEnumerable<RoleEndpointListItemDto>>(entity);
    }
}