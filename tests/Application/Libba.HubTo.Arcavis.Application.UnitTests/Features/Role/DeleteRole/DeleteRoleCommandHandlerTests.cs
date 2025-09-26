using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Features.Role.DeleteRole;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.DeleteRole;

public class DeleteRoleCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly ILogger<DeleteRoleCommandHandler> _loggerMock;
    private readonly DeleteRoleCommandHandler _sut;

    public DeleteRoleCommandHandlerTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _loggerMock = Substitute.For<ILogger<DeleteRoleCommandHandler>>();
        _sut = new DeleteRoleCommandHandler(_loggerMock, _roleRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleExist_ShouldDeleteAndSaveChanges()
    {
        var roleId = Guid.NewGuid();
        var command = new DeleteRoleCommand(roleId);
        var fakeRoleEntity = new RoleEntity { Id = roleId };

        _roleRepositoryMock.GetByIdAsync(roleId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeRoleEntity));

        await _sut.Handle(command, CancellationToken.None);

        _roleRepositoryMock.Received(1).Delete(fakeRoleEntity);

        await _roleRepositoryMock.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenRoleDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteRoleCommand(nonExistentId);

        _roleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _roleRepositoryMock.DidNotReceive().Delete(Arg.Any<RoleEntity>());
        await _roleRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());

    }
}
