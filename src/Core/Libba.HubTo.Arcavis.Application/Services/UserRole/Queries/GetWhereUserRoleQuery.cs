using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;

public record GetWhereUserRoleQuery(Expression<Func<UserRoleDto, bool>> predicate)
    : IQuery<IEnumerable<UserRoleDto>>;
