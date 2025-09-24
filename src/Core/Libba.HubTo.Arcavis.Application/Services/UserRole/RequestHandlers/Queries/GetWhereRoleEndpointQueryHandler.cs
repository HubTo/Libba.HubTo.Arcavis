using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.RequestHandlers.Queries;

public class GetWhereUserRoleQueryHandler : IQueryHandler<GetWhereUserRoleQuery, IEnumerable<UserRoleDto>?>
{
    #region Dependencies
    private readonly ILogger<GetWhereUserRoleQueryHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public GetWhereUserRoleQueryHandler(
        ILogger<GetWhereUserRoleQueryHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserRoleDto>?> Handle(GetWhereUserRoleQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting UserRole by predicate: {Predicate}",
            nameof(GetWhereUserRoleQuery),
            request.predicate);

        try
        {
            var entity = await _userRoleRepository.GetWhereAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserRoleEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<IEnumerable<UserRoleDto>?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetWhereUserRoleQuery),
                request.predicate);

            throw;
        }
    }
}