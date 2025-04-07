using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Tests.ResultContainer;

public class UnwrapExtensionsTests : ResultTestsBase
{
    [Fact]
    public void ParsingValidInt_UnwrapOrElse_ReturnExpectedValue()
    {
        // Arrange
        const int testValue = 42;
        string input = testValue.ToString();

        // Act
        var result = ParseInt32(input).UnwrapOrElse(MapErrorToResult);

        // Assert
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void ParsingValidInt_UnwrapOrDefault_ReturnExpectedValue()
    {
        // Arrange
        const int testValue = 42;
        string input = testValue.ToString();

        // Act
        var result = ParseInt32(input).UnwrapOrDefault();

        // Assert
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void ParsingValidInt_UnwrapOrException_ReturnExpectedValue()
    {
        // Arrange
        const int testValue = 42;
        string input = testValue.ToString();

        // Act
        var result = ParseInt32(input).UnwrapOrException(new ArgumentException("Invalid value"));

        // Assert
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void ParsingInvalidInt_UnwrapOrElse_ReturnMinus1()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = ParseInt32(input).UnwrapOrElse(MapErrorToResult);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void ParsingInvalidInt_UnwrapOrDefault_ReturnZero()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = ParseInt32(input).UnwrapOrDefault();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ParsingInvalidInt_UnwrapOrException_ThrowException()
    {
        // Arrange
        string input = "not a number";

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _ = ParseInt32(input).UnwrapOrException(new ArgumentException("Invalid value"));
        });
    }

    private int MapErrorToResult(string error)
    {
        return -1;
    }
}
