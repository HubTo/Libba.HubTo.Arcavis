using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.DeleteUserRole;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.DeleteUserRole;

public class DeleteUserRoleCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly DeleteUserRoleCommandHandler _sut;

    public DeleteUserRoleCommandHandlerTests()
    {
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();
        _sut = new DeleteUserRoleCommandHandler(_userRoleRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserRoleExist_ShouldDeleteAndSaveChanges()
    {
        var userRoleId = Guid.NewGuid();
        var command = new DeleteUserRoleCommand(userRoleId);
        var fakeUserRoleEntity = new UserRoleEntity { Id = userRoleId };

        _userRoleRepositoryMock.GetByIdAsync(userRoleId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeUserRoleEntity));

        await _sut.Handle(command, CancellationToken.None);

        _userRoleRepositoryMock.Received(1).Delete(fakeUserRoleEntity);
    }

    [Fact]
    public async Task Handle_WhenUserRoleDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteUserRoleCommand(nonExistentId);

        _userRoleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserRoleEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _userRoleRepositoryMock.DidNotReceive().Delete(Arg.Any<UserRoleEntity>());
    }
}
