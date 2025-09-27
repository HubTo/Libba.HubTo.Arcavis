using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.UpdateUserRole;

public class UpdateUserRoleCommandHandlerTest
{
    #region Mock Dependencies
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly UpdateUserRoleCommandHandler _sut;

    public UpdateUserRoleCommandHandlerTest()
    {
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateUserRoleCommandHandler(_userRoleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserRoleExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateUserRoleCommand(existingId, Guid.NewGuid(), Guid.NewGuid());
        var existingEntity = new UserRoleEntity { Id = existingId };

        _userRoleRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _userRoleRepositoryMock.Received(1).Update(existingEntity);

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenUserRoleDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateUserRoleCommand(nonExistentId, Guid.NewGuid(), Guid.NewGuid());

        _userRoleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<UserRoleEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"UserRole with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateUserRoleCommand>(), Arg.Any<UserRoleEntity>());
        _userRoleRepositoryMock.DidNotReceive().Update(Arg.Any<UserRoleEntity>());
    }
}
