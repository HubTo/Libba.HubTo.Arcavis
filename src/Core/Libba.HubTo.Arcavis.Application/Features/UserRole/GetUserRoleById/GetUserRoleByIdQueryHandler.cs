using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;

public class GetUserRoleByIdQueryHandler : IQueryHandler<GetUserRoleByIdQuery, UserRoleDetailDto?>
{
    #region Dependencies
    private readonly ILogger<GetUserRoleByIdQueryHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public GetUserRoleByIdQueryHandler(
        ILogger<GetUserRoleByIdQueryHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserRoleDetailDto?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting UserRole by ID: {Id}",
            nameof(GetUserRoleByIdQueryHandler),
            request.Id);

        try
        {
            var entity = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserRoleEntity matched the given id.");
                return null;
            }

            return _mapper.Map<UserRoleDetailDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetUserRoleByIdQuery),
                request.Id);

            throw;
        }
    }
}
