using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Services.User.Queries;
using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.User.RequestHandlers.Queries;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllUsersQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllUsersQueryHandler(
        ILogger<GetAllUsersQueryHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserDto>?> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting User.",
            nameof(GetAllUsersQueryHandler));

        try
        {
            var entity = await _userRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserEntity founded. Query: {Query}",
                    nameof(GetAllUsersQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved User.");

            return _mapper.Map<IEnumerable<UserDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllUsersQuery));

            throw;
        }
    }
}