using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Guid>
{
    #region Dependencies
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dal = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"User with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _userRepository.Update(dal);

        return dal.Id;
    }
}
