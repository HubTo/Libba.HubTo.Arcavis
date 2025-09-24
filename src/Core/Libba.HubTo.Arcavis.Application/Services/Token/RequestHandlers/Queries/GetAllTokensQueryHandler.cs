using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Services.Token.Queries;
using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Token.RequestHandlers.Queries;

public class GetAllTokensQueryHandler : IQueryHandler<GetAllTokensQuery, IEnumerable<TokenDto>?>
{
    #region Dependencies
    private readonly ILogger<GetAllTokensQueryHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllTokensQueryHandler(
        ILogger<GetAllTokensQueryHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<TokenDto>?> Handle(GetAllTokensQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {QueryName}: Getting Token.",
            nameof(GetAllTokensQueryHandler));

        try
        {
            var entity = await _tokenRepository.GetAllAsync(cancellationToken);

            if (entity == null)
            {
                _logger.LogWarning("No TokenEntity founded. Query: {Query}",
                    nameof(GetAllTokensQuery));
                return null;
            }

            _logger.LogInformation("Successfully retrieved Token.");

            return _mapper.Map<IEnumerable<TokenDto>>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {QueryName}.",
                nameof(GetAllTokensQuery));

            throw;
        }
    }
}