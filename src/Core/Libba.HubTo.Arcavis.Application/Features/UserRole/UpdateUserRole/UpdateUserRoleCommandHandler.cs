using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;

public class UpdateUserRoleCommandHandler : ICommandHandler<UpdateUserRoleCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateUserRoleCommandHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateUserRoleCommandHandler(
        ILogger<UpdateUserRoleCommandHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating UserRole with RoleId, UserId: {RoleId}, {UserId}:",
            nameof(UpdateUserRoleCommand),
            request.RoleId,
            request.UserId);

        try
        {
            var dal = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"UserRole with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _userRoleRepository.Update(dal);
            await _userRoleRepository.SaveAsync(cancellationToken);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for RoleId, UserId: {RoleId}, {UserId}:",
                nameof(UpdateUserRoleCommand),
                request.RoleId,
                request.UserId);

            throw;
        }
    }
}
