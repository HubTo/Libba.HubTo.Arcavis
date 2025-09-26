using FluentAssertions;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.GetAllEndpoint;

public class GetAllEndpointsQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetAllEndpointsQueryHandler _sut;

    public GetAllEndpointsQueryHandlerTests()
    {
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetAllEndpointsQueryHandler(_endpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenEndpointsExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllEndpointsQuery();

        var fakeEntityList = new List<EndpointEntity>
        {
            new EndpointEntity { Id = Guid.NewGuid(), ActionName = "Action1" },
            new EndpointEntity { Id = Guid.NewGuid(), ActionName = "Action2" }
        };
        
        var fakeDtoList = new List<EndpointListItemDto>
        {
            new EndpointListItemDto { Id = fakeEntityList[0].Id, ActionName = "Action1" },
            new EndpointListItemDto { Id = fakeEntityList[1].Id, ActionName = "Action2" }
        };

        _endpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<EndpointEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<EndpointListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoEndpointsExist_ShouldReturnNull()
    {
        var query = new GetAllEndpointsQuery();

        _endpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<EndpointEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<EndpointListItemDto>>(Arg.Any<IEnumerable<EndpointEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllEndpointsQuery();
        var emptyEntityList = new List<EndpointEntity>();
        var emptyDtoList = new List<EndpointListItemDto>();

        _endpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<EndpointEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<EndpointListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllEndpointsQuery();
        var expectedException = new InvalidOperationException("Database error");

        _endpointRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<EndpointEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
