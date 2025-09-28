using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.User.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<UserEntity>(request);

        dal.IsAccountActive = true;
        dal.IsEmailVerified = false;

        await _userRepository.AddAsync(dal, cancellationToken);

        return dal.Id;
    }
}