using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;

public record CreateTokenCommand
(
    string RefreshToken,
    DateTime ValidDate,
    Guid UserId
) : ICommand<Guid>;
