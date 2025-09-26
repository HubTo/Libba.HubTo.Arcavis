using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRole;

public class GetAllUserRolesQueryHandler : IQueryHandler<GetAllUserRolesQuery, IEnumerable<UserRoleListItemDto>?>
{
    #region Dependencies
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public GetAllUserRolesQueryHandler(
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserRoleListItemDto>?> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRoleRepository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<UserRoleListItemDto>>(entity);
    }
}