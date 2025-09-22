using System;
using System.Collections.Generic;
using System.Text;

namespace Libba.HubTo.Arcavis.Domain.Models;

/// <summary>
/// Represents the relationship between a user and a role.
/// </summary>
public class UserRoleEntity : BaseEntity
{
    /// <summary>
    /// Foreign key to the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property to the user.
    /// </summary>
    public UserEntity User { get; set; } = null!;

    /// <summary>
    /// Foreign key to the role.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Navigation property to the role.
    /// </summary>
    public RoleEntity Role { get; set; } = null!;
}
