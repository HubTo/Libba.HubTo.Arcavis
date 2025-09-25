using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDetailDto?>
{
    #region Dependencies
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;


    public GetUserByIdQueryHandler(
        ILogger<GetUserByIdQueryHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<UserDetailDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting User by ID: {Id}",
            nameof(GetUserByIdQueryHandler),
            request.Id);

        try
        {
            var entity = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserEntity matched the given id.");
                return null;
            }

            return _mapper.Map<UserDetailDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetUserByIdQuery),
                request.Id);

            throw;
        }
    }
}
