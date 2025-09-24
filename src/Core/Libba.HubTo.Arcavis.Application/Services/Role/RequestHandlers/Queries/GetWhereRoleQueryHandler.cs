using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Services.Role.Queries;
using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Role.RequestHandlers.Queries;

public class GetWhereRoleQueryHandler : IQueryHandler<GetWhereRoleQuery, IEnumerable<RoleDto>?>
{
    #region Dependencies
    private readonly ILogger<GetWhereRoleQueryHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;

    public GetWhereRoleQueryHandler(
        ILogger<GetWhereRoleQueryHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleDto>?> Handle(GetWhereRoleQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Role by predicate: {Predicate}",
            nameof(GetWhereRoleQuery),
            request.predicate);

        try
        {
            var entity = await _roleRepository.GetWhereAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<IEnumerable<RoleDto>?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetWhereRoleQuery),
                request.predicate);

            throw;
        }
    }
}