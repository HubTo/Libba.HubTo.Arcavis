using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.CreateUserRole;

public class CreateUserRoleCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly CreateUserRoleCommandHandler _sut;

    public CreateUserRoleCommandHandlerTests()
    {
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new CreateUserRoleCommandHandler(_userRoleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveUserRoleAndReturnId()
    {
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());
        var expectedId = Guid.NewGuid();
        var mappedEntity = new UserRoleEntity { Id = expectedId };

        _mapperMock.Map<UserRoleEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _userRoleRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());
        var mappedEntity = new UserRoleEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<UserRoleEntity>(command).Returns(mappedEntity);

        _userRoleRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");
    }
}
