using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;

public record GetWhereEndpointQuery(Expression<Func<EndpointDto, bool>> predicate) 
    : IQuery<IEnumerable<EndpointDto>>;
