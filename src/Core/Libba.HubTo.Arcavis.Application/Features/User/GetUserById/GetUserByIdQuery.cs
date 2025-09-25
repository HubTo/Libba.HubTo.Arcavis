using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.GetUserById;

public record GetUserByIdQuery
(
    Guid Id
) : IQuery<UserDetailDto?>;