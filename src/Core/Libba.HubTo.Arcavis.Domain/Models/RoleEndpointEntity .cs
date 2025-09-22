using System;
using System.Collections.Generic;
using System.Text;

namespace Libba.HubTo.Arcavis.Domain.Models;

/// <summary>
/// Represents the relationship between a role and an endpoint.
/// </summary>
public class RoleEndpointEntity : BaseEntity
{
    /// <summary>
    /// Foreign key to the role.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Navigation property to the role.
    /// </summary>
    public RoleEntity Role { get; set; } = null!;

    /// <summary>
    /// Foreign key to the endpoint.
    /// </summary>
    public Guid EndpointId { get; set; }

    /// <summary>
    /// Navigation property to the endpoint.
    /// </summary>
    public EndpointEntity Endpoint { get; set; } = null!;
}