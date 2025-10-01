namespace Libba.HubTo.Arcavis.Application.Features.User.SignIn;

public class SignInResponseDto
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public string RefreshToken { get; set; }

}
