namespace Libba.HubTo.Arcavis.Application.Features.Token.GetAllToken;

public class TokenListItemDto
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ValidDate { get; set; }
    public Guid UserId { get; set; }
}
