using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Commands;

public record CreateTokenCommand
(
    string RefreshToken,
    DateTime ValidDate,
    Guid UserId
) : ICommand<Guid>;
