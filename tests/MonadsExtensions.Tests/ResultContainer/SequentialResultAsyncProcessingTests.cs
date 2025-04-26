using MonadsExtensions.ResultContainer;
using MonadsExtensions.Extensions;

namespace MonadsExtensions.Tests.ResultContainer;

public class SequentialResultAsyncProcessingTests : ResultTestsBase
{
    // Sequential processing of results is a common pattern in functional programming.
    // If input is not a number - the result should be -1.
    // If input is a negative number - the result should be -1.
    // If input is a positive number and even - the result should be divided by 2.
    // If input is a positive number and odd - the result should be multiplied by 2.
    [Theory]
    [InlineData("42", 21)]
    [InlineData("21", 42)]
    [InlineData("-5", -1)]
    [InlineData("not a number", -1)]
    public async Task ValidInput_Sqr_ResultHasSqrValueAsync(string input, int expectedValue)
    {
        // Arrange
        // Act
        var result = (await ParseInt32Async(input)
                .BindAsync(ToIntermediateResult)
                .BindAsync(ProcessNegativeAsync)
                .BindAsync(ProcessMod2Async)
                .DoAsync(LogErrorAsync))
            .UnwrapOrElse(ToIntermediateResult)
            .Map(UnwrapIntermediateResult);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    protected async Task<Result<int, string>> ParseInt32Async(string input)
    {
        await Task.Yield();

        if (int.TryParse(input, out var value))
        {
            return Result.Ok(value);
        }

        return Result.Error("Invalid input");
    }

    private static async Task<IntermediateResult<int>> ProcessNegativeAsync(IntermediateResult<int> result)
    {
        await Task.Yield();

        if (result.Completed)
        {
            return result;
        }

        return result.Value < 0
            ? new IntermediateResult<int>(-1, true)
            : new IntermediateResult<int>(result.Value, false);
    }

    private static async Task<IntermediateResult<int>> ProcessMod2Async(IntermediateResult<int> result)
    {
        await Task.Yield();

        if (result.Completed)
        {
            return result;
        }

        return result.Value % 2 == 0 ?
            new IntermediateResult<int>(result.Value / 2, true) :
            new IntermediateResult<int>(result.Value * 2, true);
    }

    private static async Task LogErrorAsync<T>(Result<T, string> result)
    {
        await Task.Yield();

        if (!result.HasValue)
        {
            Console.WriteLine(result.Error);
        }
    }

    private readonly struct IntermediateResult<T>(T value, bool completed)
    {
        public T Value { get; } = value;

        public bool Completed { get; } = completed;
    }

    private static IntermediateResult<int> ToIntermediateResult(int value)
    {
        return new IntermediateResult<int>(value, false);
    }

    private static IntermediateResult<int> ToIntermediateResult(string error)
    {
        return new IntermediateResult<int>(-1, true);
    }

    private static int UnwrapIntermediateResult(IntermediateResult<int> task)
    {
        return task.Completed ? task.Value : -1;
    }
}
