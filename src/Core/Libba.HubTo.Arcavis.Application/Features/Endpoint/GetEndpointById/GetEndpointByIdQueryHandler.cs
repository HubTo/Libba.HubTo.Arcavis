using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;

public class GetEndpointByIdQueryHandler : IQueryHandler<GetEndpointByIdQuery, EndpointDetailDto?>
{
    #region Dependencies
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;

    public GetEndpointByIdQueryHandler(
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<EndpointDetailDto?> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<EndpointDetailDto?>(entity);
    }
}
