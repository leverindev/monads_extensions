using MonadsExtensions.OptionContainer;

namespace MonadsExtensions.Tests.OptionContainer;

public class UnwrapExtensionsTests : OptionTestsBase
{
    [Fact]
    public void IsEvenValidInput_UnwrapOrDefault_ReturnExpectedValue()
    {
        // Arrange
        const int testValue = 42;
        const bool expectedResult = testValue % 2 == 0;
        string input = testValue.ToString();

        // Act
        var result = IsEven(input).UnwrapOrDefault();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsEvenValidInput_UnwrapOrException_ReturnExpectedValue()
    {
        // Arrange
        const int testValue = 42;
        const bool expectedResult = testValue % 2 == 0;
        string input = testValue.ToString();

        // Act
        var result = IsEven(input).UnwrapOrException();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsEvenInvalidInput_UnwrapOrDefault_ReturnZero()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = IsEven(input).UnwrapOrDefault();

        // Assert
        Assert.Equal(false, result);
    }

    [Fact]
    public void IsEvenInvalidInput_UnwrapOrException_ThrowException()
    {
        // Arrange
        string input = "not a number";

        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _ = IsEven(input).UnwrapOrException();
        });
    }

    [Fact]
    public void IsEvenInvalidInput_UnwrapOrExceptionWithMessage_ThrowException()
    {
        // Arrange
        string input = "not a number";

        // Act
        // Assert
        Assert.Throws<Exception>(() =>
        {
            _ = IsEven(input).UnwrapOrException("Invalid input");
        });
    }

    [Fact]
    public void IsEvenInvalidInput_UnwrapOrExceptionWithException_ThrowException()
    {
        // Arrange
        string input = "not a number";

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _ = IsEven(input).UnwrapOrException(new ArgumentException("Invalid input"));
        });
    }
}
