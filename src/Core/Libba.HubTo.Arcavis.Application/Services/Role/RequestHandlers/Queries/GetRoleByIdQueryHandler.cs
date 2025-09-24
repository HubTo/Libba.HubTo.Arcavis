using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Services.Role.Queries;
using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Role.RequestHandlers.Queries;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDto?>
{
    #region Dependencies
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;


    public GetRoleByIdQueryHandler(
        ILogger<GetRoleByIdQueryHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Role by ID: {Id}",
            nameof(GetAllRolesQueryHandler),
            request.Id);

        try
        {
            var entity = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No RoleEntity matched the given id.");
                return null;
            }

            return _mapper.Map<RoleDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetRoleByIdQuery),
                request.Id);

            throw;
        }
    }
}
