using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;

public record GetWhereEndpointQuery(Expression<Func<EndpointEntity, bool>> predicate) 
    : IQuery<IEnumerable<EndpointDto>>;
