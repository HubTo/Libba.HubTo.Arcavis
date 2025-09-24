using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.RequestHandlers.Commands;

public class UpdateEndpointCommandHandler : ICommandHandler<UpdateEndpointCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateEndpointCommandHandler> _logger;
    private readonly IEndpointRepository _endpointRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateEndpointCommandHandler(
        ILogger<UpdateEndpointCommandHandler> logger,
        IEndpointRepository endpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _endpointRepository = endpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating Endpoint with Action Name: {ActionName}:",
            nameof(UpdateEndpointCommand),
            request.ActionName);

        try
        {
            var dal = await _endpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"Endpoint with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _endpointRepository.Update(dal);
            await _endpointRepository.SaveAsync();

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Action Name: {ActionName}",
                nameof(UpdateEndpointCommand),
                request.ActionName);

            throw;
        } 
    }
}
