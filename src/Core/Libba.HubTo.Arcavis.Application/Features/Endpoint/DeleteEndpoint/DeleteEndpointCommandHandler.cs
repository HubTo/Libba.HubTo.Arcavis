using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.DeleteEndpoint;

public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, bool>
{
    #region Dependencies
    private readonly IEndpointRepository _endpointRepository;

    public DeleteEndpointCommandHandler(
        IEndpointRepository endpointRepository)
    {
        _endpointRepository = endpointRepository;
    }
    #endregion

    public async Task<bool> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            return false;

        _endpointRepository.Delete(dal);

        return true;
    }
}