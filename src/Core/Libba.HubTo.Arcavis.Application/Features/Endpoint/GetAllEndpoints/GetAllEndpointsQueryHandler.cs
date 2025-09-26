using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;

public class GetAllEndpointsQueryHandler : IQueryHandler<GetAllEndpointsQuery, IEnumerable<EndpointListItemDto>?>
{
    #region Dependencies
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;

    public GetAllEndpointsQueryHandler(
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<EndpointListItemDto>?> Handle(GetAllEndpointsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _endpointRepository.GetAllAsync(cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<IEnumerable<EndpointListItemDto>>(entity);
    }
}