using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.RequestHandlers.Queries;

public class GetRoleEndpointByIdQueryHandler : IQueryHandler<GetRoleEndpointByIdQuery, RoleEndpointDto?>
{
    #region Dependencies
    private readonly ILogger<GetRoleEndpointByIdQueryHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetRoleEndpointByIdQueryHandler(
        ILogger<GetRoleEndpointByIdQueryHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleEndpointDto?> Handle(GetRoleEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting RoleEndpoint by ID: {Id}",
            nameof(GetAllRoleEndpointsQueryHandler),
            request.Id);

        try
        {
            var entity = await _roleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEndpointEntity matched the given id.");
                return null;
            }

            return _mapper.Map<RoleEndpointDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetRoleEndpointByIdQuery),
                request.Id);

            throw;
        }
    }
}
