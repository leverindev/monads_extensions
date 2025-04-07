namespace MonadsExtensions.Tests.ResultContainer;

public class GenerativeMethodsTests : ResultTestsBase
{
    [Fact]
    public void ParsingValidInt_ResultHasValue()
    {
        // Arrange
        string input = "42";

        // Act
        var result = ParseInt32(input);

        // Assert
        Assert.True(result.HasValue);
    }

    [Fact]
    public void ParsingV2ValidInt_ResultHasValue()
    {
        // Arrange
        string input = "42";

        // Act
        var result = ParseInt32V2(input);

        // Assert
        Assert.True(result.HasValue);
    }

    [Fact]
    public void ParsingInvalidInt_ResultDoNotHasValue()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = ParseInt32(input);

        // Assert
        Assert.False(result.HasValue);
    }

    [Fact]
    public void ParsingV2InvalidInt_ResultDoNotHasValue()
    {
        // Arrange
        string input = "not a number";

        // Act
        var result = ParseInt32V2(input);

        // Assert
        Assert.False(result.HasValue);
    }

    [Theory]
    [InlineData("42")]
    [InlineData("not a number")]
    public void ParsingV1V2_ResultEquals(string input)
    {
        // Arrange
        // Act
        var result1 = ParseInt32(input);
        var result2 = ParseInt32V2(input);

        // Assert
        Assert.Equal(result1, result2);
    }
}
