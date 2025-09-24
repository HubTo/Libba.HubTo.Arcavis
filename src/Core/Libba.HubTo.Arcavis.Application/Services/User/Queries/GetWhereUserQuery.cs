using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.User.Queries;

public record GetWhereUserQuery(Expression<Func<UserEntity, bool>> predicate)
    : IQuery<IEnumerable<UserDto>>;
