using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Features.User.DeleteUser;
using Libba.HubTo.Arcavis.Application.Features.User.GetAllUsers;
using Libba.HubTo.Arcavis.Application.Features.User.GetUserById;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Libba.HubTo.Arcavis.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region Dependencies
    private readonly IArcavisCQRS _arcavisCQRS;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IArcavisCQRS arcavisCQRS, 
        ILogger<UserController> logger)
    {
        _arcavisCQRS = arcavisCQRS;
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(Id);

        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var success = await _arcavisCQRS.SendAsync(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
