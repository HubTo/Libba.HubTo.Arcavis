using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.UpdateUser;

public class UpdateUserCommandHandlerTest
{
    #region Mock Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly UpdateUserCommandHandler _sut;

    public UpdateUserCommandHandlerTest()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateUserCommandHandler(_userRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateUserCommand(existingId, "TestName", "TestDescription");
        var existingEntity = new UserEntity { Id = existingId };

        _userRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _userRepositoryMock.Received(1).Update(existingEntity);

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateUserCommand(nonExistentId, "TestName", "TestDescription");

        _userRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<UserEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"User with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateUserCommand>(), Arg.Any<UserEntity>());
        _userRepositoryMock.DidNotReceive().Update(Arg.Any<UserEntity>());
    }
}
