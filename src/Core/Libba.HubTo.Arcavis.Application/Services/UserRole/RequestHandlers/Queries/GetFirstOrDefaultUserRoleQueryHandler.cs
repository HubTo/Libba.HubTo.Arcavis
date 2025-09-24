using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.RequestHandlers.Queries;

public class GetFirstOrDefaultUserRoleQueryHandler : IQueryHandler<GetFirstOrDefaultUserRoleQuery, UserRoleDto?>
{
    #region Dependencies
    private readonly ILogger<GetFirstOrDefaultUserRoleQueryHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public GetFirstOrDefaultUserRoleQueryHandler(
        ILogger<GetFirstOrDefaultUserRoleQueryHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserRoleDto?> Handle(GetFirstOrDefaultUserRoleQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting UserRole by predicate: {Predicate}",
            nameof(GetFirstOrDefaultUserRoleQuery),
            request.predicate);

        try
        {
            var entity = await _userRoleRepository.GetFirstOrDefaultAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserRoleEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<UserRoleDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetFirstOrDefaultUserRoleQuery),
                request.predicate);

            throw;
        }
    }
}
