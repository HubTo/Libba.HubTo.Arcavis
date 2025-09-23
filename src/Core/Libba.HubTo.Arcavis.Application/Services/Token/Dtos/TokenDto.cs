namespace Libba.HubTo.Arcavis.Application.Services.Token.Dtos;

public class TokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ValidDate { get; set; }
    public Guid UserId { get; set; }
}
