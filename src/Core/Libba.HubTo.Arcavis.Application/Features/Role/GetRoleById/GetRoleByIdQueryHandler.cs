using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.GetRoleById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDetailDto?>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;

    public GetRoleByIdQueryHandler(
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<RoleDetailDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<RoleDetailDto?>(entity);
    }
}
