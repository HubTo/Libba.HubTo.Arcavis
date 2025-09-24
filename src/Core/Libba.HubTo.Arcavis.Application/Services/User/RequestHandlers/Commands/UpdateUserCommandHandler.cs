using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Services.User.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.User.RequestHandlers.Commands;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateUserCommandHandler(
        ILogger<UpdateUserCommandHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating User with Email: {Email}:",
            nameof(UpdateUserCommand),
            request.Email);

        try
        {
            var dal = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"User with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _userRepository.Update(dal);
            await _userRepository.SaveAsync();

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Email: {Email}",
                nameof(UpdateUserCommand),
                request.Email);

            throw;
        } 
    }
}
