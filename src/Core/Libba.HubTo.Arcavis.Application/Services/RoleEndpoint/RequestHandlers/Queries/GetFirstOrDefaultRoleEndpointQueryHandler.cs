using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.RequestHandlers.Queries;

public class GetFirstOrDefaultRoleEndpointQueryHandler : IQueryHandler<GetFirstOrDefaultRoleEndpointQuery, RoleEndpointDto?>
{
    #region Dependencies
    private readonly ILogger<GetFirstOrDefaultRoleEndpointQueryHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;

    public GetFirstOrDefaultRoleEndpointQueryHandler(
        ILogger<GetFirstOrDefaultRoleEndpointQueryHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleEndpointDto?> Handle(GetFirstOrDefaultRoleEndpointQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting RoleEndpoint by predicate: {Predicate}",
            nameof(GetFirstOrDefaultRoleEndpointQuery),
            request.predicate);

        try
        {
            var entity = await _roleEndpointRepository.GetFirstOrDefaultAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEndpointEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<RoleEndpointDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetFirstOrDefaultRoleEndpointQuery),
                request.predicate);

            throw;
        }
    }
}
