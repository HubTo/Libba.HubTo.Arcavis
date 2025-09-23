using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.User.Commands;

public record CreateUserCommand
(
    string Email,
    string PhoneCode,
    string PhoneNumber,
    string Password,
    bool IsAccountActive,
    bool IsEmailVerified
) : ICommand<Guid>;
