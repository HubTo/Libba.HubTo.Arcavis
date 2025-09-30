using Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;
using Libba.HubTo.Arcavis.Application.Features.Role.GetRoleById;
using Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;
using Libba.HubTo.Arcavis.Application.Features.Role.DeleteRole;
using Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Libba.HubTo.Arcavis.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    #region Dependencies
    private readonly IArcavisCQRS _arcavisCQRS;
    private readonly ILogger<RoleController> _logger;

    public RoleController(
        IArcavisCQRS arcavisCQRS, 
        ILogger<RoleController> logger)
    {
        _arcavisCQRS = arcavisCQRS;
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetRoleByIdQuery(Id);

        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(id);
        var success = await _arcavisCQRS.SendAsync(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
