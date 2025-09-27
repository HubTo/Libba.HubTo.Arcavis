using Libba.HubTo.Arcavis.Application.Adapters;
using FluentAssertions;
using NSubstitute;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Adapters.Mappings;

public class ArcavisMapperTests
{
    #region Dependencies
    private readonly IMapper _autoMapperMock;
    private readonly ArcavisMapper _sut;

    public ArcavisMapperTests()
    {
        _autoMapperMock = Substitute.For<IMapper>();
        _sut = new ArcavisMapper(_autoMapperMock);
    }
    #endregion

    [Fact]
    public void Map_WhenCalledWithSource_ShouldCallAutoMapperMapWithSameSource()
    {
        var sourceObject = new { Name = "Test" };
        var expectedDestinationObject = new { Name = "Mapped Test" };

        _autoMapperMock.Map<object>(sourceObject).Returns(expectedDestinationObject);

        var result = _sut.Map<object>(sourceObject);

        _autoMapperMock.Received(1).Map<object>(sourceObject);

        result.Should().Be(expectedDestinationObject);
    }

    [Fact]
    public void Map_WhenCalledWithSourceAndDestination_ShouldCallAutoMapperMapWithSameParameters()
    {
        var sourceObject = new { Value = 10 };
        var destinationObject = new { Value = 0 };
        var expectedResultObject = new { Value = 10 }; // Map'lenmiş hali

        _autoMapperMock.Map(sourceObject, destinationObject).Returns(expectedResultObject);

        var result = _sut.Map(sourceObject, destinationObject);

        _autoMapperMock.Received(1).Map(sourceObject, destinationObject);
        result.Should().Be(expectedResultObject);
    }

    [Fact]
    public void MapEnumerable_WhenCalledWithSourceEnumerable_ShouldCallAutoMapperMapCorrectly()
    {
        var sourceList = new List<object> { new { Id = 1 }, new { Id = 2 } };
        var expectedDestinationList = new List<object> { new { Id = 1 }, new { Id = 2 } };

        _autoMapperMock.Map<IEnumerable<object>>(sourceList).Returns(expectedDestinationList);

        var result = _sut.MapEnumerable<object>(sourceList);

        _autoMapperMock.Received(1).Map<IEnumerable<object>>(sourceList);
        result.Should().BeSameAs(expectedDestinationList);
    }
}

