using FluentAssertions;
using ProductCatalog.Application.ValueObjects;
using Xunit;

namespace ProductCatalog.Tests.UnitTests;

public class NameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("this is too long name of the product")]
    public void should_not_allow_to_create_invalid_name(string name) =>
        Assert.Throws<InvalidNameException>(() => (Name) name);

    [Fact]
    public void can_create_name_from_string()
    {
        var value = "some name";
        var result = (Name) value;

        result.Value.Should().Be(value);
        result.ToString().Should().Be(value);

        var resultAsString = (string) result;
        resultAsString.Should().Be(value);
    }
}