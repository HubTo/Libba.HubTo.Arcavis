using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDetailDto?>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserDetailDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<UserDetailDto?>(entity);
    }
}
