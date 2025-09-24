using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Services.Role.Queries;
using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Role.RequestHandlers.Queries;

public class GetFirstOrDefaultRoleQueryHandler : IQueryHandler<GetFirstOrDefaultRoleQuery, RoleDto?>
{
    #region Dependencies
    private readonly ILogger<GetFirstOrDefaultRoleQueryHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;

    public GetFirstOrDefaultRoleQueryHandler(
        ILogger<GetFirstOrDefaultRoleQueryHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleDto?> Handle(GetFirstOrDefaultRoleQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Role by predicate: {Predicate}",
            nameof(GetFirstOrDefaultRoleQuery),
            request.predicate);

        try
        {
            var entity = await _roleRepository.GetFirstOrDefaultAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<RoleDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetFirstOrDefaultRoleQuery),
                request.predicate);

            throw;
        }
    }
}
