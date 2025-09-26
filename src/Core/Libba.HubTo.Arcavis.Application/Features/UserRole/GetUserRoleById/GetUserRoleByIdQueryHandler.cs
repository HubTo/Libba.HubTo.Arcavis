using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;

public class GetUserRoleByIdQueryHandler : IQueryHandler<GetUserRoleByIdQuery, UserRoleDetailDto?>
{
    #region Dependencies
    private readonly ILogger<GetUserRoleByIdQueryHandler> _logger;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IArcavisMapper _mapper;


    public GetUserRoleByIdQueryHandler(
        ILogger<GetUserRoleByIdQueryHandler> logger,
        IUserRoleRepository userRoleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserRoleDetailDto?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _userRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<UserRoleDetailDto?>(entity);
    }
}
