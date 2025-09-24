using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Models;
using System.Linq.Expressions;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;

public record GetFirstOrDefaultEndpointQuery(Expression<Func<EndpointEntity, bool>> predicate)
    : IQuery<EndpointDto?>;
