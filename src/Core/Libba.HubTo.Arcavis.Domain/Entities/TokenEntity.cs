namespace Libba.HubTo.Arcavis.Domain.Entities;

/// <summary>
/// User Authentication Tokens
/// </summary>
public class TokenEntity : BaseEntity
{
    #region Columns
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// When Refresh Token Is Not Be Able To Work.
    /// </summary>
    public DateTime ValidDate { get; set; }
    #endregion

    #region Relations
    /// <summary>
    /// Users
    /// </summary>
    public UserEntity Users { get; set; }
    #endregion
}
