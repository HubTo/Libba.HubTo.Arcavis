using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;

public record GetWhereUserRoleQuery(Expression<Func<UserRoleEntity, bool>> predicate)
    : IQuery<IEnumerable<UserRoleDto>>;
