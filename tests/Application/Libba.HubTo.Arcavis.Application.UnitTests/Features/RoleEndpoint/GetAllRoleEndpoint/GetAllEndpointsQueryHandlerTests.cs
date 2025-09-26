using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.GetAllRoleEndpoint;

public class GetAllRoleEndpointsQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly ILogger<GetAllRoleEndpointsQueryHandler> _loggerMock;
    private readonly GetAllRoleEndpointsQueryHandler _sut;

    public GetAllRoleEndpointsQueryHandlerTests()
    {
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _loggerMock = Substitute.For<ILogger<GetAllRoleEndpointsQueryHandler>>();
        _sut = new GetAllRoleEndpointsQueryHandler(_loggerMock, _roleEndpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleEndpointsExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllRoleEndpointsQuery();

        var fakeEntityList = new List<RoleEndpointEntity>
        {
            new RoleEndpointEntity { Id = Guid.NewGuid(), RoleId = Guid.NewGuid() },
            new RoleEndpointEntity { Id = Guid.NewGuid(), RoleId = Guid.NewGuid() }
        };
        
        var fakeDtoList = new List<RoleEndpointListItemDto>
        {
            new RoleEndpointListItemDto { Id = fakeEntityList[0].Id, RoleId = Guid.NewGuid() },
            new RoleEndpointListItemDto { Id = fakeEntityList[1].Id, RoleId = Guid.NewGuid() }
        };

        _roleEndpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEndpointEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<RoleEndpointListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoRoleEndpointsExist_ShouldReturnNull()
    {
        var query = new GetAllRoleEndpointsQuery();

        _roleEndpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEndpointEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<RoleEndpointListItemDto>>(Arg.Any<IEnumerable<RoleEndpointEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllRoleEndpointsQuery();
        var emptyEntityList = new List<RoleEndpointEntity>();
        var emptyDtoList = new List<RoleEndpointListItemDto>();

        _roleEndpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<RoleEndpointEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<RoleEndpointListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllRoleEndpointsQuery();
        var expectedException = new InvalidOperationException("Database error");

        _roleEndpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<RoleEndpointEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
