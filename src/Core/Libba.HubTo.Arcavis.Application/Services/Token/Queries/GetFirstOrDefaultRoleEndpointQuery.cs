using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Queries;

public record GetFirstOrDefaultTokenQuery(Expression<Func<TokenEntity, bool>> predicate)
    : IQuery<TokenDto?>;