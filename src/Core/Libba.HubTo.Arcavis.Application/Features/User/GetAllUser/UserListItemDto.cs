namespace Libba.HubTo.Arcavis.Application.Features.User.GetAllUser;

public class UserListItemDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsAccountActive { get; set; }
    public bool IsEmailVerified { get; set; }
}
