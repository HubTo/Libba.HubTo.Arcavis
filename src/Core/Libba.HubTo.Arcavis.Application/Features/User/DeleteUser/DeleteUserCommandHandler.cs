using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    #endregion

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var dal = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal == null)
            return false;

        _userRepository.Delete(dal);

        return true;
    }
}