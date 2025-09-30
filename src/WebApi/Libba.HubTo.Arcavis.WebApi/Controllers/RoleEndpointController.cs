using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetRoleEndpointById;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.DeleteRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Libba.HubTo.Arcavis.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleEndpointController : ControllerBase
{
    #region Dependencies
    private readonly IArcavisCQRS _arcavisCQRS;
    private readonly ILogger<RoleEndpointController> _logger;

    public RoleEndpointController(
        IArcavisCQRS arcavisCQRS, 
        ILogger<RoleEndpointController> logger)
    {
        _arcavisCQRS = arcavisCQRS;
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllRoleEndpointsQuery();
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetRoleEndpointByIdQuery(Id);
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleEndpointCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoleEndpointCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleEndpointCommand(id);
        var success = await _arcavisCQRS.SendAsync(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
