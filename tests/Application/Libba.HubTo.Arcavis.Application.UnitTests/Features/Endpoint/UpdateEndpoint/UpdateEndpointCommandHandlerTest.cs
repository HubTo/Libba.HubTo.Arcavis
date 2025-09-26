using Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.UpdateEndpoint;

public class UpdateEndpointCommandHandlerTest
{
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly ILogger<UpdateEndpointCommandHandler> _loggerMock;
    private readonly UpdateEndpointCommandHandler _sut;

    public UpdateEndpointCommandHandlerTest()
    {
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _loggerMock = Substitute.For<ILogger<UpdateEndpointCommandHandler>>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateEndpointCommandHandler(_loggerMock, _endpointRepositoryMock, _mapperMock);
    }

    [Fact]
    public async Task Handle_WhenEndpointExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateEndpointCommand(existingId, "UpdatedModule", "UpdatedController", "UpdatedAction", 
                                                Domain.Enums.HttpVerb.POST, "Updated.Namespace");
        var existingEntity = new EndpointEntity { Id = existingId };

        _endpointRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _endpointRepositoryMock.Received(1).Update(existingEntity);

        await _endpointRepositoryMock.Received(1).SaveAsync();

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenEndpointDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateEndpointCommand(nonExistentId, "UpdatedModule", "UpdatedController", "UpdatedAction", 
                                                Domain.Enums.HttpVerb.POST, "Updated.Namespace");

        _endpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<EndpointEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"Endpoint with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateEndpointCommand>(), Arg.Any<EndpointEntity>());
        _endpointRepositoryMock.DidNotReceive().Update(Arg.Any<EndpointEntity>());

        await _endpointRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
}
