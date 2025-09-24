using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.RequestHandlers.Commands;

public class DeleteUserRoleCommandHandler : ICommandHandler<DeleteUserRoleCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteUserRoleCommandHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteUserRoleCommandHandler(
        ILogger<DeleteUserRoleCommandHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting UserRole with ID: {Id}:",
            nameof(DeleteUserRoleCommand),
            request.Id);

        try
        {
            var dal = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _userRoleRepository.Delete(dal);
            await _userRoleRepository.SaveAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteUserRoleCommand),
                request.Id);

            throw;
        }
    }
}