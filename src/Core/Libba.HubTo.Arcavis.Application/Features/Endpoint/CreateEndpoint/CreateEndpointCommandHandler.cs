using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;

public class CreateEndpointCommandHandler : ICommandHandler<CreateEndpointCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateEndpointCommandHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;


    public CreateEndpointCommandHandler(
        ILogger<CreateEndpointCommandHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating Endpoint with Action Name: {ActionName}:", 
            nameof(CreateEndpointCommand), 
            request.ActionName);

        try
        {
            var dal = _mapper.Map<EndpointEntity>(request);

            await _endpointRepository.AddAsync(dal, cancellationToken);
            await _endpointRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created Endpoint with ID: {EndpointId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Action Name: {ActionName}",
                nameof(CreateEndpointCommand),
                request.ActionName);

            throw;
        }
    }
}