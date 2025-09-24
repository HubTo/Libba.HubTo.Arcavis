using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.RequestHandlers.Queries;

public class GetWhereRoleEndpointQueryHandler : IQueryHandler<GetWhereRoleEndpointQuery, IEnumerable<RoleEndpointDto>?>
{
    #region Dependencies
    private readonly ILogger<GetWhereRoleEndpointQueryHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;

    public GetWhereRoleEndpointQueryHandler(
        ILogger<GetWhereRoleEndpointQueryHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleEndpointDto>?> Handle(GetWhereRoleEndpointQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting RoleEndpoint by predicate: {Predicate}",
            nameof(GetWhereRoleEndpointQuery),
            request.predicate);

        try
        {
            var entity = await _roleEndpointRepository.GetWhereAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEndpointEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<IEnumerable<RoleEndpointDto>?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetWhereRoleEndpointQuery),
                request.predicate);

            throw;
        }
    }
}