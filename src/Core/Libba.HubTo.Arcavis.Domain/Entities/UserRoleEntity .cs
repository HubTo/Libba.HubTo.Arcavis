namespace Libba.HubTo.Arcavis.Domain.Entities;

/// <summary>
/// Represents the relationship between a user and a role.
/// </summary>
public class UserRoleEntity : BaseEntity
{
    #region Columns
    /// <summary>
    /// Foreign key to the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Foreign key to the role.
    /// </summary>
    public Guid RoleId { get; set; }
    #endregion

    #region Relations
    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public UserEntity User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the role.
    /// </summary>
    public RoleEntity Role { get; set; } = null!;
    #endregion
}
