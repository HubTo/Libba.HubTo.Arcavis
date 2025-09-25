using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.DeleteRoleEndpoint;

public class DeleteRoleEndpointCommandHandler : ICommandHandler<DeleteRoleEndpointCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteRoleEndpointCommandHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteRoleEndpointCommandHandler(
        ILogger<DeleteRoleEndpointCommandHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting RoleEndpoint with ID: {Id}:",
            nameof(DeleteRoleEndpointCommand),
            request.Id);

        try
        {
            var dal = await _roleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _roleEndpointRepository.Delete(dal);
            await _roleEndpointRepository.SaveAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteRoleEndpointCommand),
                request.Id);

            throw;
        }
    }
}