using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using Libba.HubTo.Arcavis.Common.Helpers.Argon2;

namespace Libba.HubTo.Arcavis.Application.Features.User.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public CreateUserCommandHandler(
        ILogger<CreateUserCommandHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating User with Email: {Email}:", 
            nameof(CreateUserCommand), 
            request.Email);

        try
        {
            var dal = _mapper.Map<UserEntity>(request);

            dal.IsAccountActive = true;
            dal.IsEmailVerified = false;
            dal.PasswordHash = Argon2Helper.HashPassword(request.Password);

            await _userRepository.AddAsync(dal, cancellationToken);
            await _userRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created User with ID: {UserId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Email: {Email}",
                nameof(CreateUserCommand),
                request.Email);

            throw;
        }
    }
}