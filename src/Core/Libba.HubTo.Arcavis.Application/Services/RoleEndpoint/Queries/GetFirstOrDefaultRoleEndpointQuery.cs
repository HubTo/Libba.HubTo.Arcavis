using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;

public record GetFirstOrDefaultRoleEndpointQuery(Expression<Func<RoleEndpointDto, bool>> Predicate)
    : IQuery<RoleEndpointDto?>;