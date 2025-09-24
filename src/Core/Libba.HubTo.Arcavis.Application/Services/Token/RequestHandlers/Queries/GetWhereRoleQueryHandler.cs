using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Services.Token.Queries;
using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Token.RequestHandlers.Queries;

public class GetWhereTokenQueryHandler : IQueryHandler<GetWhereTokenQuery, IEnumerable<TokenDto>?>
{
    #region Dependencies
    private readonly ILogger<GetWhereTokenQueryHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;

    public GetWhereTokenQueryHandler(
        ILogger<GetWhereTokenQueryHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<TokenDto>?> Handle(GetWhereTokenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Token by predicate: {Predicate}",
            nameof(GetWhereTokenQuery),
            request.predicate);

        try
        {
            var entity = await _tokenRepository.GetWhereAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No TokenEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<IEnumerable<TokenDto>?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetWhereTokenQuery),
                request.predicate);

            throw;
        }
    }
}