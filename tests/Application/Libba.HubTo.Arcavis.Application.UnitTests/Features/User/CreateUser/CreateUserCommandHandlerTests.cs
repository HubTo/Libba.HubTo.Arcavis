using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.CreateUser;

public class CreateUserCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new CreateUserCommandHandler(_userRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveUserAndReturnId()
    {
        var command = new CreateUserCommand("TestPhoneCode", "TestPhoneNumber");
        var expectedId = Guid.NewGuid();
        var mappedEntity = new UserEntity { Id = expectedId };

        _mapperMock.Map<UserEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _userRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var command = new CreateUserCommand("TestPhoneCode", "TestPhoneNumber");
        var mappedEntity = new UserEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<UserEntity>(command).Returns(mappedEntity);

        _userRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");
    }
}
