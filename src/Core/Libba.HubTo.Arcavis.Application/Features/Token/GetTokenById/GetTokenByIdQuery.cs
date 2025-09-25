using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;

public record GetTokenByIdQuery
(
    Guid Id
) : IQuery<TokenDetailDto?>;