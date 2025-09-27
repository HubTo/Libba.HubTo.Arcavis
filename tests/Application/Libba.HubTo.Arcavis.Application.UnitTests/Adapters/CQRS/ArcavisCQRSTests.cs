using Libba.HubTo.Arcavis.Application.Adapters;
using FluentAssertions;
using NSubstitute;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Adapters.CQRS;

public class ArcavisCQRSTests
{
    private readonly IMediator _mediatorMock;
    private readonly ArcavisCQRS _sut;

    public ArcavisCQRSTests()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _sut = new ArcavisCQRS(_mediatorMock);
    }

    [Fact]
    public async Task SendAsync_WithRequestReturningResult_ShouldCallMediatorSendAndReturnResult()
    {
        var fakeQuery = new FakeQuery();
        var expectedResult = "Test Result";

        _mediatorMock.Send(fakeQuery, Arg.Any<CancellationToken>()).Returns(expectedResult);

        var actualResult = await _sut.SendAsync(fakeQuery, CancellationToken.None);

        await _mediatorMock.Received(1).Send(fakeQuery, Arg.Any<CancellationToken>());

        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task SendAsync_WithRequestNotReturningResult_ShouldCallMediatorSend()
    {
        var fakeCommand = new FakeCommand();

        await _sut.SendAsync(fakeCommand, CancellationToken.None);

        await _mediatorMock.Received(1).Send(fakeCommand, Arg.Any<CancellationToken>());
    }

    // 3. PublishAsync Testi (Domain Event'leri gibi bildirimler için)
    [Fact]
    public async Task PublishAsync_WithNotification_ShouldCallMediatorPublish()
    {
        var fakeNotification = new FakeNotification();

        await _sut.PublishAsync(fakeNotification, CancellationToken.None);

        await _mediatorMock.Received(1).Publish(fakeNotification, Arg.Any<CancellationToken>());
    }
}

public class FakeQuery : IRequest<string> { }
public class FakeCommand : IRequest { }
public class FakeNotification : INotification { }
