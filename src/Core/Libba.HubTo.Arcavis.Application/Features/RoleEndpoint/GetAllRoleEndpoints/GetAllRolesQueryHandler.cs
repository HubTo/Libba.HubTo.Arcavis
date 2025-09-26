using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;

public class GetAllRoleEndpointsQueryHandler : IQueryHandler<GetAllRoleEndpointsQuery, IEnumerable<RoleEndpointListItemDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllRoleEndpointsQueryHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllRoleEndpointsQueryHandler(
        ILogger<GetAllRoleEndpointsQueryHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleEndpointListItemDto>?> Handle(GetAllRoleEndpointsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting RoleEndpoint.",
            nameof(GetAllRoleEndpointsQueryHandler));

        try
        {
            var entity = await _roleEndpointRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEndpointEntity founded. Query: {Query}",
                    nameof(GetAllRoleEndpointsQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved RoleEndpoint.");

            return _mapper.Map<IEnumerable<RoleEndpointListItemDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllRoleEndpointsQuery));

            throw;
        }
    }
}