using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Services.Token.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Token.RequestHandlers.Commands;

public class UpdateTokenCommandHandler : ICommandHandler<UpdateTokenCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateTokenCommandHandler> _logger;
    private readonly ITokenRepository _tokenRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateTokenCommandHandler(
        ILogger<UpdateTokenCommandHandler> logger,
        ITokenRepository tokenRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating Token with User ID: {UserId}:",
            nameof(UpdateTokenCommand),
            request.UserId);

        try
        {
            var dal = await _tokenRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"Token with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _tokenRepository.Update(dal);
            await _tokenRepository.SaveAsync(cancellationToken);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for User ID: {UserId}",
                nameof(UpdateTokenCommand),
                request.UserId);

            throw;
        } 
    }
}
