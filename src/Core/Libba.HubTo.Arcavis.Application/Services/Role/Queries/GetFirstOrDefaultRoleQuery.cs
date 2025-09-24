using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Role.Queries;

public record GetFirstOrDefaultRoleQuery(Expression<Func<RoleEntity, bool>> predicate)
    : IQuery<RoleDto?>;