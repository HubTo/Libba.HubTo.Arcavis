using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.DeleteRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.DeleteRoleEndpoint;

public class DeleteRoleEndpointCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly ILogger<DeleteRoleEndpointCommandHandler> _loggerMock;
    private readonly DeleteRoleEndpointCommandHandler _sut;

    public DeleteRoleEndpointCommandHandlerTests()
    {
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();
        _loggerMock = Substitute.For<ILogger<DeleteRoleEndpointCommandHandler>>();
        _sut = new DeleteRoleEndpointCommandHandler(_loggerMock, _roleEndpointRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleEndpointExist_ShouldDeleteAndSaveChanges()
    {
        var roleEndpointId = Guid.NewGuid();
        var command = new DeleteRoleEndpointCommand(roleEndpointId);
        var fakeRoleEndpointEntity = new RoleEndpointEntity { Id = roleEndpointId };

        _roleEndpointRepositoryMock.GetByIdAsync(roleEndpointId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeRoleEndpointEntity));

        await _sut.Handle(command, CancellationToken.None);

        _roleEndpointRepositoryMock.Received(1).Delete(fakeRoleEndpointEntity);

        await _roleEndpointRepositoryMock.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenRoleEndpointDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteRoleEndpointCommand(nonExistentId);

        _roleEndpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEndpointEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _roleEndpointRepositoryMock.DidNotReceive().Delete(Arg.Any<RoleEndpointEntity>());
        await _roleEndpointRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());

    }
}
