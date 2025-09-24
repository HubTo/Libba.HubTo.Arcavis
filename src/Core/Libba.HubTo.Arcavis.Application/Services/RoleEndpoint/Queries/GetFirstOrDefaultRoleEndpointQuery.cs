using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;

public record GetFirstOrDefaultRoleEndpointQuery(Expression<Func<RoleEndpointEntity, bool>> predicate)
    : IQuery<RoleEndpointDto?>;