using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;

public class UpdateEndpointCommandHandler : ICommandHandler<UpdateEndpointCommand, Guid>
{
    #region Dependencies
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateEndpointCommandHandler(
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"Endpoint with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _endpointRepository.Update(dal);

        return dal.Id;
    }
}
