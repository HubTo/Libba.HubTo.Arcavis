using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;
using Libba.HubTo.Arcavis.Application.Features.Token.GetAllToken;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;

public class GetTokenByIdQueryHandler : IQueryHandler<GetTokenByIdQuery, TokenDetailDto?>
{
    #region Dependencies
    private readonly ILogger<GetTokenByIdQueryHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public GetTokenByIdQueryHandler(
        ILogger<GetTokenByIdQueryHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<TokenDetailDto?> Handle(GetTokenByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Token by ID: {Id}",
            nameof(GetAllTokensQueryHandler),
            request.Id);

        try
        {
            var entity = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No TokenEntity matched the given id.");
                return null;
            }

            return _mapper.Map<TokenDetailDto?>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName} by ID: {Id}.",
                nameof(GetTokenByIdQuery),
                request.Id);

            throw;
        }
    }
}
