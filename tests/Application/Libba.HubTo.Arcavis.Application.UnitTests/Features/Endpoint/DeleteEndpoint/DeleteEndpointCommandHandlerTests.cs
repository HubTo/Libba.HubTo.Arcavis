using Libba.HubTo.Arcavis.Application.Features.Endpoint.DeleteEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.DeleteEndpoint;

public class DeleteEndpointCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly ILogger<DeleteEndpointCommandHandler> _loggerMock;
    private readonly DeleteEndpointCommandHandler _sut;

    public DeleteEndpointCommandHandlerTests()
    {
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _loggerMock = Substitute.For<ILogger<DeleteEndpointCommandHandler>>();
        _sut = new DeleteEndpointCommandHandler(_loggerMock, _endpointRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenEndpointExist_ShouldDeleteAndSaveChanges()
    {
        var endpointId = Guid.NewGuid();
        var command = new DeleteEndpointCommand(endpointId);
        var fakeEndpointEntity = new EndpointEntity { Id = endpointId };

        _endpointRepositoryMock.GetByIdAsync(endpointId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeEndpointEntity));

        await _sut.Handle(command, CancellationToken.None);

        _endpointRepositoryMock.Received(1).Delete(fakeEndpointEntity);

        await _endpointRepositoryMock.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenEndpointDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteEndpointCommand(nonExistentId);

        _endpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<EndpointEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _endpointRepositoryMock.DidNotReceive().Delete(Arg.Any<EndpointEntity>());
        await _endpointRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());

    }
}
