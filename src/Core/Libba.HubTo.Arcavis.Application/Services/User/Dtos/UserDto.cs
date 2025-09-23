using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;

namespace Libba.HubTo.Arcavis.Application.Services.User.Dtos;

public class UserDto
{
    public string Email { get; set; } = string.Empty;
    public string PhoneCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsAccountActive { get; set; }
    public bool IsEmailVerified { get; set; }
}
