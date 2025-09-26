using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.UpdateToken;

public class UpdateTokenCommandHandler : ICommandHandler<UpdateTokenCommand, Guid>
{
    #region Dependencies
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateTokenCommandHandler(
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        var dal = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"Token with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _tokenRepository.Update(dal);

        return dal.Id;
    }
}
