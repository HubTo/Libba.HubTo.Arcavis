using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetAllToken;

public record GetAllTokensQuery() : IQuery<IEnumerable<TokenListItemDto>>;