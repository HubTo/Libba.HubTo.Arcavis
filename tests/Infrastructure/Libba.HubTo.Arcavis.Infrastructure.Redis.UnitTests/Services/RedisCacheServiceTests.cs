using Libba.HubTo.Arcavis.Infrastructure.Redis.Services;
using StackExchange.Redis;
using System.Text.Json;
using NSubstitute;
using FluentAssertions;

namespace Libba.HubTo.Arcavis.Infrastructure.Redis.UnitTests.Services;

public class RedisCacheServiceTests
{
    private readonly IDatabase _databaseMock;
    private readonly RedisCacheService _sut;

    public RedisCacheServiceTests()
    {
        _databaseMock = Substitute.For<IDatabase>();
        var multiplexerMock = Substitute.For<IConnectionMultiplexer>();
        multiplexerMock.GetDatabase(Arg.Any<int>(), Arg.Any<object>()).Returns(_databaseMock);

        _sut = new RedisCacheService(multiplexerMock);
    }

    [Fact]
    public async Task GetAsync_WhenKeyExists_ShouldDeserializeAndReturnValue()
    {
        var key = "my-key";
        var expectedObject = new TestData { Id = 1, Name = "Test" };
        var json = JsonSerializer.Serialize(expectedObject);

        _databaseMock.StringGetAsync(key, CommandFlags.None).Returns(json);

        var result = await _sut.GetAsync<TestData>(key, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedObject);
    }

    [Fact]
    public async Task SetAsync_WhenCalled_ShouldSerializeAndCallStringSetAsync()
    {
        var key = "my-key";
        var value = new TestData { Id = 1, Name = "Test" };
        var expectedJson = JsonSerializer.Serialize(value);
        var expiry = TimeSpan.FromMinutes(5);

        await _sut.SetAsync(key, value, expiry, CancellationToken.None);

        await _databaseMock.Received(1).StringSetAsync(key, expectedJson, expiry);
    }

    private class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
