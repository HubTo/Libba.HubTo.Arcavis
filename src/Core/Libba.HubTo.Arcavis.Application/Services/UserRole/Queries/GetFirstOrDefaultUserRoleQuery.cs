using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;

public record GetFirstOrDefaultUserRoleQuery(Expression<Func<UserRoleDto, bool>> Predicate)
    : IQuery<UserRoleDto?>;