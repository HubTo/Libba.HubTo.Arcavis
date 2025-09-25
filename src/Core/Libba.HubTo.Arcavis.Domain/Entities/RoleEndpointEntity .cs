namespace Libba.HubTo.Arcavis.Domain.Entities;

/// <summary>
/// Represents the relationship between a role and an endpoint.
/// </summary>
public class RoleEndpointEntity : BaseEntity
{
    #region Columns
    /// <summary>
    /// Foreign key to the role.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Foreign key to the endpoint.
    /// </summary>
    public Guid EndpointId { get; set; }
    #endregion

    #region Relations
    /// <summary>
    /// Navigation property to the role.
    /// </summary>
    public RoleEntity Role { get; set; } = null!;

    /// <summary>
    /// Navigation property to the endpoint.
    /// </summary>
    public EndpointEntity Endpoint { get; set; } = null!;
    #endregion
}