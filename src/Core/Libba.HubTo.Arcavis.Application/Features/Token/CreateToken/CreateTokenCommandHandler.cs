using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;

public class CreateTokenCommandHandler : ICommandHandler<CreateTokenCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateTokenCommandHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public CreateTokenCommandHandler(
        ILogger<CreateTokenCommandHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _tokenRepository = tokenRepository;
    }
    #endregion

    public async Task<Guid> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating Token with User ID: {UserId}:", 
            nameof(CreateTokenCommand), 
            request.UserId);

        try
        {
            var dal = _mapper.Map<TokenEntity>(request);

            await _tokenRepository.AddAsync(dal, cancellationToken);
            await _tokenRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created Token with ID: {TokenId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for User ID: {UserId}",
                nameof(CreateTokenCommand),
                request.UserId);

            throw;
        }
    }
}