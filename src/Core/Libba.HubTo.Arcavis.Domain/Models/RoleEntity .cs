using System;
using System.Collections.Generic;
using System.Text;

namespace Libba.HubTo.Arcavis.Domain.Models;

/// <summary>
/// Represents a role that can be assigned to users and used for authorization.
/// </summary>
public class RoleEntity : BaseEntity
{
    /// <summary>
    /// The name of the role.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the role.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the endpoints assigned to this role.
    /// </summary>
    public virtual ICollection<RoleEndpointEntity> RoleEndpoints { get; set; } = new List<RoleEndpointEntity>();

    /// <summary>
    /// Navigation property to the users assigned to this role.
    /// </summary>
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
}
