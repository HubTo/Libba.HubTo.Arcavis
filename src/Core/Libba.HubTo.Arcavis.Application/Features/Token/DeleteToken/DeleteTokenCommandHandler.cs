using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.DeleteToken;

public class DeleteTokenCommandHandler : ICommandHandler<DeleteTokenCommand, bool>
{
    #region Dependencies
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteTokenCommandHandler(
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        var dal = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

        _tokenRepository.Delete(dal);

        return true;
    }
}