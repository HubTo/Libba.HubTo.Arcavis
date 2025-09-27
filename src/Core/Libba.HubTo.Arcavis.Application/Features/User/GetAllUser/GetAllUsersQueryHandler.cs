using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.GetAllUser;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserListItemDto>?>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserListItemDto>?> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRepository.GetAllAsync(cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<IEnumerable<UserListItemDto>>(entity);
    }
}