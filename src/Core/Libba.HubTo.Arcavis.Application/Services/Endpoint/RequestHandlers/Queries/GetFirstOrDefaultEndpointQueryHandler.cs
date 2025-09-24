using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.RequestHandlers.Queries;

public class GetFirstOrDefaultEndpointQueryHandler : IQueryHandler<GetFirstOrDefaultEndpointQuery, EndpointDto?>
{
    #region Dependencies
    private readonly ILogger<GetFirstOrDefaultEndpointQueryHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;

    public GetFirstOrDefaultEndpointQueryHandler(
        ILogger<GetFirstOrDefaultEndpointQueryHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<EndpointDto?> Handle(GetFirstOrDefaultEndpointQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Endpoint by predicate: {Predicate}",
            nameof(GetFirstOrDefaultEndpointQuery),
            request.predicate);

        try
        {
            var entity = await _endpointRepository.GetFirstOrDefaultAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No EndpointEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<EndpointDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetFirstOrDefaultEndpointQuery),
                request.predicate);

            throw;
        }
    }
}
