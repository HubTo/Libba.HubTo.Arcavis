using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRoles;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.GetAllUserRole;

public class GetAllUserRolesQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetAllUserRolesQueryHandler _sut;

    public GetAllUserRolesQueryHandlerTests()
    {
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetAllUserRolesQueryHandler(_userRoleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserRolesExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllUserRolesQuery();

        var fakeEntityList = new List<UserRoleEntity>
        {
            new UserRoleEntity { Id = Guid.NewGuid(), UserId = Guid.NewGuid() },
            new UserRoleEntity { Id = Guid.NewGuid(), UserId = Guid.NewGuid() }
        };
        
        var fakeDtoList = new List<UserRoleListItemDto>
        {
            new UserRoleListItemDto { Id = fakeEntityList[0].Id, UserId = Guid.NewGuid() },
            new UserRoleListItemDto { Id = fakeEntityList[1].Id, UserId = Guid.NewGuid() }
        };

        _userRoleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserRoleEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<UserRoleListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoUserRolesExist_ShouldReturnNull()
    {
        var query = new GetAllUserRolesQuery();

        _userRoleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserRoleEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<UserRoleListItemDto>>(Arg.Any<IEnumerable<UserRoleEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllUserRolesQuery();
        var emptyEntityList = new List<UserRoleEntity>();
        var emptyDtoList = new List<UserRoleListItemDto>();

        _userRoleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<UserRoleEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<UserRoleListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllUserRolesQuery();
        var expectedException = new InvalidOperationException("Database error");

        _userRoleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<UserRoleEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
