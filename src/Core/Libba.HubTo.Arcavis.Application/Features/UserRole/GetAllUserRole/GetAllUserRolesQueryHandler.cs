using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRole;

public class GetAllUserRolesQueryHandler : IQueryHandler<GetAllUserRolesQuery, IEnumerable<UserRoleListItemDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllUserRolesQueryHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllUserRolesQueryHandler(
        ILogger<GetAllUserRolesQueryHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserRoleListItemDto>?> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting UserRole.",
            nameof(GetAllUserRolesQueryHandler));

        try
        {
            var entity = await _userRoleRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserRoleEntity founded. Query: {Query}",
                    nameof(GetAllUserRolesQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved UserRole.");

            return _mapper.Map<IEnumerable<UserRoleListItemDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllUserRolesQuery));

            throw;
        }
    }
}