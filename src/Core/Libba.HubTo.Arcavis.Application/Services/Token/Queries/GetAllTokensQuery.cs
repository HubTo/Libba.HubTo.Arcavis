using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Queries;

public record GetAllTokensQuery() : IQuery<List<TokenDto>>;