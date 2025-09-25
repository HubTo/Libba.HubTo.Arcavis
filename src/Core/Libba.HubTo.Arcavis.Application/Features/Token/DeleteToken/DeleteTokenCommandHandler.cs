using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Token.DeleteToken;

public class DeleteTokenCommandHandler : ICommandHandler<DeleteTokenCommand, bool>
{
    #region Dependencies
    private readonly ILogger<DeleteTokenCommandHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public DeleteTokenCommandHandler(
        ILogger<DeleteTokenCommandHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<bool> Handle(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Deleting Token with ID: {Id}:",
            nameof(DeleteTokenCommand),
            request.Id);

        try
        {
            var dal = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                return false;
            }

            _tokenRepository.Delete(dal);
            await _tokenRepository.SaveAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for ID: {Id}",
                nameof(DeleteTokenCommand),
                request.Id);

            throw;
        }
    }
}