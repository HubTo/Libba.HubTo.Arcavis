using FluentAssertions;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.GetEndpointById;

public class GetEndpointByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly ILogger<GetEndpointByIdQueryHandler> _loggerMock;
    private readonly GetEndpointByIdQueryHandler _sut;

    public GetEndpointByIdQueryHandlerTests()
    {
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _loggerMock = Substitute.For<ILogger<GetEndpointByIdQueryHandler>>();
        _sut = new GetEndpointByIdQueryHandler(_loggerMock, _endpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenEndpointWithGivenIdExists_ShouldReturnMappedDto()
    {
        var endpointId = Guid.NewGuid();
        var query = new GetEndpointByIdQuery(endpointId);

        var fakeEntity = new EndpointEntity { Id = endpointId, ActionName = "GetInvoiceById" };

        var fakeDto = new EndpointDetailDto { Id = endpointId, ActionName = "GetInvoiceById" };

        _endpointRepositoryMock.GetByIdAsync(endpointId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<EndpointEntity?>(fakeEntity));

        _mapperMock.Map<EndpointDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(endpointId);
    }

    [Fact]
    public async Task Handle_WhenEndpointWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetEndpointByIdQuery(nonExistentId);

        _endpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<EndpointEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<EndpointDetailDto?>(Arg.Any<EndpointEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var endpointId = Guid.NewGuid();
        var query = new GetEndpointByIdQuery(endpointId);
        var expectedException = new InvalidOperationException("Database error");

        _endpointRepositoryMock.GetByIdAsync(endpointId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<EndpointEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
