using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;

public class CreateUserRoleCommandHandler : ICommandHandler<CreateUserRoleCommand, Guid>
{
    #region Dependencies
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;

    public CreateUserRoleCommandHandler(
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<UserRoleEntity>(request);

        await _userRoleRepository.AddAsync(dal, cancellationToken);

        return dal.Id;
    }
}