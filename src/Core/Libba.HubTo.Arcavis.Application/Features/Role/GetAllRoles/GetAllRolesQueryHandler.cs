using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleListItemDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllRolesQueryHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllRolesQueryHandler(
        ILogger<GetAllRolesQueryHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleListItemDto>?> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Role.",
            nameof(GetAllRolesQueryHandler));

        try
        {
            var entity = await _roleRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEntity founded. Query: {Query}",
                    nameof(GetAllRolesQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved Role.");

            return _mapper.Map<IEnumerable<RoleListItemDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllRolesQuery));

            throw;
        }
    }
}