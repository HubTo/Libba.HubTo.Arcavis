using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;

public record GetWhereRoleEndpointQuery(Expression<Func<RoleEndpointDto, bool>> predicate)
    : IQuery<IEnumerable<RoleEndpointDto>>;
