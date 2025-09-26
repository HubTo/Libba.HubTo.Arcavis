using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;

public class CreateEndpointCommandHandler : ICommandHandler<CreateEndpointCommand, Guid>
{
    #region Dependencies
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;

    public CreateEndpointCommandHandler(
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<EndpointEntity>(request);

        await _endpointRepository.AddAsync(dal, cancellationToken);

        return dal.Id;
    }
}