using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetRoleEndpointById;

public class GetRoleEndpointByIdQueryHandler : IQueryHandler<GetRoleEndpointByIdQuery, RoleEndpointDetailDto?>
{
    #region Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetRoleEndpointByIdQueryHandler(
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleEndpointDetailDto?> Handle(GetRoleEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _roleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<RoleEndpointDetailDto?>(entity);
    }
}
