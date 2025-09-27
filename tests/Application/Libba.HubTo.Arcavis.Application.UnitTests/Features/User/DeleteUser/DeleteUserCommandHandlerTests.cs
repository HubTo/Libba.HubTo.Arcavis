using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.DeleteUser;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.DeleteUser;

public class DeleteUserCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly DeleteUserCommandHandler _sut;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _sut = new DeleteUserCommandHandler(_userRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserExist_ShouldDeleteAndSaveChanges()
    {
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);
        var fakeUserEntity = new UserEntity { Id = userId };

        _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeUserEntity));

        await _sut.Handle(command, CancellationToken.None);

        _userRepositoryMock.Received(1).Delete(fakeUserEntity);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteUserCommand(nonExistentId);

        _userRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _userRepositoryMock.DidNotReceive().Delete(Arg.Any<UserEntity>());
    }
}
