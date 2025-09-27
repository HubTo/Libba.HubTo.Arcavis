using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.GetUserById;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetUserByIdQueryHandler _sut;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetUserByIdQueryHandler(_userRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserWithGivenIdExists_ShouldReturnMappedDto()
    {
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery(userId);

        var fakeEntity = new UserEntity { Id = userId, PhoneNumber = "GetInvoiceById" };

        var fakeDto = new UserDetailDto { Id = userId, PhoneNumber = "GetInvoiceById" };

        _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserEntity?>(fakeEntity));

        _mapperMock.Map<UserDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(userId);
    }

    [Fact]
    public async Task Handle_WhenUserWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetUserByIdQuery(nonExistentId);

        _userRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<UserDetailDto?>(Arg.Any<UserEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var userId = Guid.NewGuid();
        var query = new GetUserByIdQuery(userId);
        var expectedException = new InvalidOperationException("Database error");

        _userRepositoryMock.GetByIdAsync(userId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<UserEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
