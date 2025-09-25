namespace Libba.HubTo.Arcavis.Domain.Entities;

/// <summary>
/// User Authentication Profiles
/// </summary>
public class UserEntity : BaseEntity
{
    #region Constructor
    public UserEntity()
    {
        TokenEntities = new HashSet<TokenEntity>();
        UserRoleEntities = new HashSet<UserRoleEntity>();
    }
    #endregion

    #region Columns
    /// <summary>
    /// Email Address
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Phone Country Code
    /// </summary>
    public string PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// Phone Number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// HAshed Password With Argon2
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// Is Account Active
    /// </summary>
    public bool IsAccountActive { get; set; }

    /// <summary>
    /// Is Email Verified
    /// </summary>
    public bool IsEmailVerified { get; set; }
    #endregion

    #region Relations
    /// <summary>
    /// Token Entity
    /// </summary>
    public virtual ICollection<TokenEntity> TokenEntities { get; set; }

    /// <summary>
    /// User Roles
    /// </summary>
    public virtual ICollection<UserRoleEntity> UserRoleEntities { get; set; }
    #endregion
}
