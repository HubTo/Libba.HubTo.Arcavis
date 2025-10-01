namespace Libba.HubTo.Arcavis.Application.Models.Token;

public class SignInModel
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public string RefreshToken { get; set; }
}
