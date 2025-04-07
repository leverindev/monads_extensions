using MonadsExtensions.ResultContainer;

namespace MonadsExtensions.Tests.ResultContainer;

public class BindExtensionsTests : ResultTestsBase
{
    [Fact]
    public void ValidInput_Sqr_ResultHasSqrValue()
    {
        // Arrange
        const int inputValue = 10;
        const int expectedValue = inputValue * inputValue;
        string strInput = inputValue.ToString();

        // Act
        var result = ParseInt32(strInput).Bind(Sqr);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void InvalidInput_Sqr_ResultHasSqrValue()
    {
        // Arrange
        string strInput = "not a number";

        // Act
        var result = ParseInt32(strInput).Bind(Sqr);

        // Assert
        Assert.False(result.HasValue);
    }

    private static int Sqr(int value)
    {
        return value * value;
    }
}
