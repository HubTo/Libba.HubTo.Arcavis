using Libba.HubTo.Arcavis.Domain.Enums;

namespace Libba.HubTo.Arcavis.Domain.Models;

/// <summary>
/// Represents an API endpoint used for authorization and module management.
/// </summary>
public class EndpointEntity : BaseEntity
{
    #region Columns
    /// <summary>
    /// The module this endpoint belongs to. (e.g., "Auth", "User")
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the controller this endpoint belongs to.
    /// </summary>
    public string ControllerName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the action or method of the endpoint.
    /// </summary>
    public string ActionName { get; set; } = string.Empty;

    /// <summary>
    /// The HTTP verb of the endpoint (GET, POST, PUT, DELETE, etc.).
    /// </summary>
    public HttpVerb HttpVerb { get; set; }

    /// <summary>
    /// The namespace of the controller this endpoint belongs to.
    /// </summary>
    public string Namespace { get; set; } = string.Empty;
    #endregion

    #region Relations
    /// <summary>
    /// Navigation property to the roles that have access to this endpoint.
    /// </summary>
    public virtual ICollection<RoleEndpointEntity> RoleEndpoints { get; set; } = new List<RoleEndpointEntity>();
    #endregion
}