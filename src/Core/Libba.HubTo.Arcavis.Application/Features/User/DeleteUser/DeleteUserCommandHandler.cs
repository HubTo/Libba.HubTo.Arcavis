using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var dal = await _userRepository.GetByIdAsync(request.Id, cancellationToken);


        _userRepository.Delete(dal);

        return true;
    }
}