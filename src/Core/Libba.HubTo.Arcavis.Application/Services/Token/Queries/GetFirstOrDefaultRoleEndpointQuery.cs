using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Queries;

public record GetFirstOrDefaultTokenQuery(Expression<Func<TokenDto, bool>> Predicate)
    : IQuery<TokenDto?>;