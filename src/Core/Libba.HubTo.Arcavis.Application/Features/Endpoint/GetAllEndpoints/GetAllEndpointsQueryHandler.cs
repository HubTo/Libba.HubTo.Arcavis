using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;

public class GetAllEndpointsQueryHandler : IQueryHandler<GetAllEndpointsQuery, IEnumerable<EndpointListItemDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllEndpointsQueryHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllEndpointsQueryHandler(
        ILogger<GetAllEndpointsQueryHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<EndpointListItemDto>?> Handle(GetAllEndpointsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Endpoint.",
            nameof(GetAllEndpointsQueryHandler));

        try
        {
            var entity = await _endpointRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No EndpointEntity founded. Query: {Query}",
                    nameof(GetAllEndpointsQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved Endpoint.");

            return _mapper.Map<IEnumerable<EndpointListItemDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllEndpointsQuery));

            throw;
        }
    }
}