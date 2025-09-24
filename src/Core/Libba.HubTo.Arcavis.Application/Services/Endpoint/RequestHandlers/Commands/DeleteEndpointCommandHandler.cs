using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.RequestHandlers.Commands;

public class DeleteEndpointCommandHandler : ICommandHandler<DeleteEndpointCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteEndpointCommandHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteEndpointCommandHandler(
        ILogger<DeleteEndpointCommandHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting Endpoint with ID: {Id}:",
            nameof(DeleteEndpointCommand),
            request.Id);

        try
        {
            var dal = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _endpointRepository.Delete(dal);
            await _endpointRepository.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteEndpointCommand),
                request.Id);

            throw;
        }
    }
}