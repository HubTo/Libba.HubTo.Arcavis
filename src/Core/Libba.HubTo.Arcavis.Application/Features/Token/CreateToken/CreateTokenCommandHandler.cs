using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;

public class CreateTokenCommandHandler : ICommandHandler<CreateTokenCommand, Guid>
{
    #region Dependencies
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public CreateTokenCommandHandler(
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _mapper = mapper;
        _tokenRepository = tokenRepository;
    }
    #endregion

    public async Task<Guid> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var dal = _mapper.Map<TokenEntity>(request);

        await _tokenRepository.AddAsync(dal, cancellationToken);

        return dal.Id;
    }
}