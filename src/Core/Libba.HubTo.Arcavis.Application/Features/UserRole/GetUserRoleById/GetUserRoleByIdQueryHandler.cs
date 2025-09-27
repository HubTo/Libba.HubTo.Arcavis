using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;

public class GetUserRoleByIdQueryHandler : IQueryHandler<GetUserRoleByIdQuery, UserRoleDetailDto?>
{
    #region Dependencies
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public GetUserRoleByIdQueryHandler(
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserRoleDetailDto?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<UserRoleDetailDto?>(entity);
    }
}
