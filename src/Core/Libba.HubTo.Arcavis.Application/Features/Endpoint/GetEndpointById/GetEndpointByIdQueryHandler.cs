using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, EndpointDetailDto?>
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

    public async Task<EndpointDetailDto?> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Endpoint by ID: {Id}",
            nameof(GetEndpointByIdQueryHandler),
            request.Id);

        try
        {
            var entity = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No EndpointEntity matched the given id.");
                return null;
            }

            return _mapper.Map<EndpointDetailDto?>(entity);
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
