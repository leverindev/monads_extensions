using MonadsExtensions.OptionContainer;

namespace MonadsExtensions.Tests.OptionContainer;

public class GenerativeMethodsTests : OptionTestsBase
{
    [Fact]
    public void IsEvenValidInput_ResultHasValue()
    {
        // Arrange
        string input = "42";

        // Act
        var result = IsEven(input);

        // Assert
        Assert.True(result.HasValue);
    }

    [Fact]
    public void IsEvenV2ValidInput_ResultHasValue()
    {
        // Arrange
        string input = "42";

        // Act
        var result = IsEvenV2(input);

        // Assert
        Assert.True(result.HasValue);
    }

    [Fact]
    public void IsEvenInvalidInput_ResultDoNotHasValue()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = IsEven(input);

        // Assert
        Assert.False(result.HasValue);
        Assert.Equal(result, Option.None);
    }

    [Fact]
    public void IsEvenV2InvalidInput_ResultDoNotHasValue()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = IsEvenV2(input);

        // Assert
        Assert.False(result.HasValue);
        Assert.Equal(result, Option.None);
    }

    [Theory]
    [InlineData("42")]
    [InlineData("not a number")]
    public void IsEvenV1V2_ResultEquals(string input)
    {
        // Arrange
        // Act
        var result1 = IsEven(input);
        var result2 = IsEvenV2(input);

        // Assert
        Assert.Equal(result1, result2);
    }
}
