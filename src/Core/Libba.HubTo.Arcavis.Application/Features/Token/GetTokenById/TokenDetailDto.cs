namespace Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;

public class TokenDetailDto
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ValidDate { get; set; }
    public Guid UserId { get; set; }
}
