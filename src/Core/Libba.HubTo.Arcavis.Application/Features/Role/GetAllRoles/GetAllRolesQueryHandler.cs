using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleListItemDto>?>
{
    #region Dependencies
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllRolesQueryHandler(
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RoleListItemDto>?> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var entity = await _roleRepository.GetAllAsync(cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<IEnumerable<RoleListItemDto>>(entity);
    }
}