using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using Libba.HubTo.Arcavis.Domain.Enums;
using NSubstitute.ReceivedExtensions;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.CreateEndpoint;

public class CreateEndpointCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly ILogger<CreateEndpointCommandHandler> _loggerMock;
    private readonly CreateEndpointCommandHandler _sut;

    public CreateEndpointCommandHandlerTests()
    {
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _loggerMock = Substitute.For<ILogger<CreateEndpointCommandHandler>>();
        _sut = new CreateEndpointCommandHandler(_loggerMock, _endpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveEndpointAndReturnId()
    {
        var command = new CreateEndpointCommand("TestModule", "TestController", "TestAction", HttpVerb.GET, "Test.Namespace");
        var expectedId = Guid.NewGuid();
        var mappedEntity = new EndpointEntity { Id = expectedId };

        _mapperMock.Map<EndpointEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _endpointRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        await _endpointRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var command = new CreateEndpointCommand("TestModule", "TestController", "TestAction", HttpVerb.GET, "Test.Namespace");
        var mappedEntity = new EndpointEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<EndpointEntity>(command).Returns(mappedEntity);

        _endpointRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");

        await _endpointRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
}
