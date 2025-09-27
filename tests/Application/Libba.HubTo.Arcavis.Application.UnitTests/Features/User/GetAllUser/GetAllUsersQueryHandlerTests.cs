using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.GetAllUsers;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.GetAllUser;

public class GetAllUsersQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetAllUsersQueryHandler _sut;

    public GetAllUsersQueryHandlerTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetAllUsersQueryHandler(_userRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUsersExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllUsersQuery();

        var fakeEntityList = new List<UserEntity>
        {
            new UserEntity { Id = Guid.NewGuid(), PhoneCode = "Action1" },
            new UserEntity { Id = Guid.NewGuid(), PhoneCode = "Action2" }
        };
        
        var fakeDtoList = new List<UserListItemDto>
        {
            new UserListItemDto { Id = fakeEntityList[0].Id, PhoneCode = "Action1" },
            new UserListItemDto { Id = fakeEntityList[1].Id, PhoneCode = "Action2" }
        };

        _userRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<UserListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoUsersExist_ShouldReturnNull()
    {
        var query = new GetAllUsersQuery();

        _userRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<UserListItemDto>>(Arg.Any<IEnumerable<UserEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllUsersQuery();
        var emptyEntityList = new List<UserEntity>();
        var emptyDtoList = new List<UserListItemDto>();

        _userRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<UserListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllUsersQuery();
        var expectedException = new InvalidOperationException("Database error");

        _userRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<UserEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
