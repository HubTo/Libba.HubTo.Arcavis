using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Features.Token.GetAllToken;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;

public class GetTokenByIdQueryHandler : IQueryHandler<GetTokenByIdQuery, TokenDetailDto?>
{
    #region Dependencies
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public GetTokenByIdQueryHandler(
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<TokenDetailDto?> Handle(GetTokenByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<TokenDetailDto?>(entity);
    }
}
