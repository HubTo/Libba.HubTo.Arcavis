using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.RequestHandlers.Commands;

public class CreateUserRoleCommandHandler : ICommandHandler<CreateUserRoleCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateUserRoleCommandHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public CreateUserRoleCommandHandler(
        ILogger<CreateUserRoleCommandHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating UserRole with RoleId, UserId: {RoleId}, {UserId}:", 
            nameof(CreateUserRoleCommand), 
            request.RoleId,
            request.UserId);

        try
        {
            var dal = _mapper.Map<UserRoleEntity>(request);

            await _userRoleRepository.AddAsync(dal, cancellationToken);
            await _userRoleRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created UserRole with ID: {UserRoleId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for RoleId, UserId: {RoleId}, {UserId}:",
                nameof(CreateUserRoleCommand),
                request.RoleId,
                request.UserId);

            throw;
        }
    }
}