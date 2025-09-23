using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Role.Queries;

public record GetWhereRoleQuery(Expression<Func<RoleDto, bool>> predicate)
    : IQuery<IEnumerable<RoleDto>>;
