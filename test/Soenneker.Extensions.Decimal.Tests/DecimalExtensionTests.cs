using AwesomeAssertions;
using Soenneker.Tests.Unit;
using Xunit;

namespace Soenneker.Extensions.Decimal.Tests;

public sealed class DecimalExtensionTests : UnitTest
{
    [Theory]
    [InlineData(0, false, "$0.00")]
    [InlineData(0, true, "$0")]
    [InlineData(1234.56, false, "$1,234.56")]
    [InlineData(1234.56, true, "$1,235")]
    [InlineData(1234, false, "$1,234.00")]
    [InlineData(1234, true, "$1,234")]
    [InlineData(0.99, false, "$0.99")]
    [InlineData(0.99, true, "$1")]
    [InlineData(-1234.56, false, "-$1,234.56")]
    [InlineData(-1234.56, true, "-$1,235")]
    [InlineData(-0.99, false, "-$0.99")]
    [InlineData(-0.99, true, "-$1")]
    [InlineData(1000000, false, "$1,000,000.00")]
    [InlineData(1000000, true, "$1,000,000")]
    public void ToCurrencyDisplay_ShouldReturnExpectedResults(decimal value, bool excludePlaces, string expected)
    {
        // Act
        string result = value.ToCurrencyDisplay(excludePlaces);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToCurrencyDisplay_ShouldHandleFractionalTruncatingProperly()
    {
        // Arrange
        const decimal value = 1234.5678m;

        // Act
        string result = value.ToCurrencyDisplay();

        // Assert
        result.Should().Be("$1,234.57");
    }

    [Theory]
    [InlineData(0.33, "33%")]
    [InlineData(0.335, "33.5%")]
    [InlineData(0.3333, "33.33%")]
    [InlineData(0.3333555, "33.34%")]
    [InlineData(0.5, "50%")]
    [InlineData(0.99, "99%")]
    [InlineData(1, "100%")]
    public void ToPercentDisplay_CorrectsScaling(decimal input, string expected)
    {
        // Act
        string result = input.ToPercentDisplay();

        // Assert
        result.Should().Be(expected);
    }
}
