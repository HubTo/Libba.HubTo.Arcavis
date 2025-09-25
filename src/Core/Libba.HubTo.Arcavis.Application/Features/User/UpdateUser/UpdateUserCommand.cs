using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;

public record UpdateUserCommand
(
    Guid Id,
    string Email,
    string PhoneCode,
    string PhoneNumber,
    string Password,
    bool IsAccountActive,
    bool IsEmailVerified
) : ICommand<Guid>;