using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Services.Token.Queries;
using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Token.RequestHandlers.Queries;

public class GetFirstOrDefaultTokenQueryHandler : IQueryHandler<GetFirstOrDefaultTokenQuery, TokenDto?>
{
    #region Dependencies
    private readonly ILogger<GetFirstOrDefaultTokenQueryHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;

    public GetFirstOrDefaultTokenQueryHandler(
        ILogger<GetFirstOrDefaultTokenQueryHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<TokenDto?> Handle(GetFirstOrDefaultTokenQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Token by predicate: {Predicate}",
            nameof(GetFirstOrDefaultTokenQuery),
            request.predicate);

        try
        {
            var entity = await _tokenRepository.GetFirstOrDefaultAsync(request.predicate, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No TokenEntity matched the given predicate.");
                return null;
            }

            return _mapper.Map<TokenDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by predicate: {Predicate}.",
                nameof(GetFirstOrDefaultTokenQuery),
                request.predicate);

            throw;
        }
    }
}
