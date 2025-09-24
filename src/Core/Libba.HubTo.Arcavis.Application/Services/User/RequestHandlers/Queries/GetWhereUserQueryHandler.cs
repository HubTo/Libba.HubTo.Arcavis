using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Services.User.Queries;
using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.User.RequestHandlers.Queries;

public class GetWhereUserQueryHandler : IQueryHandler<GetWhereUserQuery, IEnumerable<UserDto>?>
{
    #region Dependencies
    private readonly ILogger<GetWhereUserQueryHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IArcavisMapper _mapper;

    public GetWhereUserQueryHandler(
        ILogger<GetWhereUserQueryHandler> logger,
        IUserRepository userRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UserDto>?> Handle(GetWhereUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting User by predicate: {Predicate}",
            nameof(GetWhereUserQuery),
            request.predicate);

        try
        {
            var entity = await _userRepository.GetWhereAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No UserEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<IEnumerable<UserDto>?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetWhereUserQuery),
                request.predicate);

            throw;
        }
    }
}