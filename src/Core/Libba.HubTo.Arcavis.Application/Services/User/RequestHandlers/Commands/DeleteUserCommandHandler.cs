using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Services.User.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.User.RequestHandlers.Commands;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteUserCommandHandler(
        ILogger<DeleteUserCommandHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting User with ID: {Id}:",
            nameof(DeleteUserCommand),
            request.Id);

        try
        {
            var dal = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _userRepository.Delete(dal);
            await _userRepository.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteUserCommand),
                request.Id);

            throw;
        }
    }
}