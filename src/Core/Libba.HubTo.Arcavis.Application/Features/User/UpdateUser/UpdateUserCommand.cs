using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;

public record UpdateUserCommand
(
    Guid Id,
    string PhoneCode,
    string PhoneNumber
) : ICommand<Guid>;