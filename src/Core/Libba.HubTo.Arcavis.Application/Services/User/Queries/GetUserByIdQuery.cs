using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.User.Queries;

public record GetUserByIdQuery
(
    Guid Id
) : IQuery<UserDto?>;