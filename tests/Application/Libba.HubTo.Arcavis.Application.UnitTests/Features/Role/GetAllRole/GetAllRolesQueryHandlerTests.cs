using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.GetAllRole;

public class GetAllRolesQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetAllRolesQueryHandler _sut;

    public GetAllRolesQueryHandlerTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetAllRolesQueryHandler(_roleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRolesExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllRolesQuery();

        var fakeEntityList = new List<RoleEntity>
        {
            new RoleEntity { Id = Guid.NewGuid(), Name = "Action1" },
            new RoleEntity { Id = Guid.NewGuid(), Name = "Action2" }
        };
        
        var fakeDtoList = new List<RoleListItemDto>
        {
            new RoleListItemDto { Id = fakeEntityList[0].Id, Name = "Action1" },
            new RoleListItemDto { Id = fakeEntityList[1].Id, Name = "Action2" }
        };

        _roleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<RoleListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoRolesExist_ShouldReturnNull()
    {
        var query = new GetAllRolesQuery();

        _roleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<RoleListItemDto>>(Arg.Any<IEnumerable<RoleEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllRolesQuery();
        var emptyEntityList = new List<RoleEntity>();
        var emptyDtoList = new List<RoleListItemDto>();

        _roleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<RoleListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllRolesQuery();
        var expectedException = new InvalidOperationException("Database error");

        _roleRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<RoleEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
