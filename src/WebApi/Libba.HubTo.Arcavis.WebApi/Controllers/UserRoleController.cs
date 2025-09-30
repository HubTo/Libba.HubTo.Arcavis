using Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;
using Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRoles;
using Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.DeleteUserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Libba.HubTo.Arcavis.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRoleController : ControllerBase
{
    #region Dependencies
    private readonly IArcavisCQRS _arcavisCQRS;
    private readonly ILogger<UserRoleController> _logger;

    public UserRoleController(
        IArcavisCQRS arcavisCQRS, 
        ILogger<UserRoleController> logger)
    {
        _arcavisCQRS = arcavisCQRS;
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllUserRolesQuery();
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetUserRoleByIdQuery(Id);
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRoleCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserRoleCommand(id);
        var success = await _arcavisCQRS.SendAsync(command, cancellationToken);

        if (!success)
            return NotFound();

        return Ok(success);
    }
}
