using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.DeleteEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Libba.HubTo.Arcavis.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EndpointController : ControllerBase
{
    #region Dependencies
    private readonly IArcavisCQRS _arcavisCQRS;
    private readonly ILogger<EndpointController> _logger;

    public EndpointController(
        IArcavisCQRS arcavisCQRS, 
        ILogger<EndpointController> logger)
    {
        _arcavisCQRS = arcavisCQRS;
        _logger = logger;
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllEndpointsQuery();
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid Id, CancellationToken cancellationToken)
    {
        var query = new GetEndpointByIdQuery(Id);
        var users = await _arcavisCQRS.SendAsync(query, cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEndpointCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEndpointCommand command, CancellationToken cancellationToken)
    {
        var id = await _arcavisCQRS.SendAsync(command, cancellationToken);

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteEndpointCommand(id);
        var success = await _arcavisCQRS.SendAsync(command, cancellationToken);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
