using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Role.DeleteRole;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteRoleCommandHandler> _logger;
    private readonly IRoleRepository _roleRepository;


    public DeleteRoleCommandHandler(
        ILogger<DeleteRoleCommandHandler> logger,
        IRoleRepository roleRepository)
    {
        _logger = logger;
        _roleRepository = roleRepository;
    }
    #endregion

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting Role with ID: {Id}:",
            nameof(DeleteRoleCommand),
            request.Id);

        try
        {
            var dal = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _roleRepository.Delete(dal);
            await _roleRepository.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteRoleCommand),
                request.Id);

            throw;
        }
    }
}