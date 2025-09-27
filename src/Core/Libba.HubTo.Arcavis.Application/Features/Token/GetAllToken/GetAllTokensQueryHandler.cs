using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetAllTokens;

public class GetAllTokensQueryHandler : IQueryHandler<GetAllTokensQuery, IEnumerable<TokenListItemDto>?>
{
    #region Dependencies
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public GetAllTokensQueryHandler(
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<TokenListItemDto>?> Handle(GetAllTokensQuery request, CancellationToken cancellationToken)
    {
        var entity = await _tokenRepository.GetAllAsync(cancellationToken);

        if (entity == null)
            return null;

        return _mapper.Map<IEnumerable<TokenListItemDto>>(entity);
    }
}