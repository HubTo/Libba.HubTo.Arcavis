using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.RequestHandlers.Queries;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, EndpointDto?>
{
    #region Dependencies
    private readonly ILogger<GetEndpointByIdQueryHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;


    public GetEndpointByIdQueryHandler(
        ILogger<GetEndpointByIdQueryHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<EndpointDto?> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Endpoint by ID: {Id}",
            nameof(GetAllEndpointsQueryHandler),
            request.Id);

        try
        {
            var entity = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No EndpointEntity matched the given id.");
                return null;
            }

            return _mapper.Map<EndpointDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetEndpointByIdQuery),
                request.Id);

            throw;
        }
    }
}
