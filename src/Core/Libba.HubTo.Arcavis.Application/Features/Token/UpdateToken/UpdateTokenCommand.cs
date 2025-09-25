using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.UpdateToken;

public record UpdateTokenCommand
(
    Guid Id,
    string RefreshToken,
    DateTime ValidDate,
    Guid UserId
) : ICommand<Guid>;