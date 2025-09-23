using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.User.Queries;

public record GetWhereUserQuery(Expression<Func<UserDto, bool>> predicate)
    : IQuery<IEnumerable<UserDto>>;
