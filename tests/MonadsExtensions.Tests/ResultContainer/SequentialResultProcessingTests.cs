using MonadsExtensions.ResultContainer;
using MonadsExtensions.Extensions;

namespace MonadsExtensions.Tests.ResultContainer;

public class SequentialResultProcessingTests : ResultTestsBase
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
    public void ValidInput_Sqr_ResultHasSqrValue(string input, int expectedValue)
    {
        // Arrange
        // Act
        var result = ParseInt32(input)
            .Bind(ToTask)
            .Bind(ProcessNegative)
            .Bind(ProcessMod2)
            .Do(LogError)
            .UnwrapOrElse(ToTask)
            .Map(UnwrapTask);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    private static IntermediateTask<int> ToTask(int value)
    {
        return new IntermediateTask<int>(value, false);
    }

    private static IntermediateTask<int> ToTask(string error)
    {
        return new IntermediateTask<int>(-1, true);
    }

    private static IntermediateTask<int> ProcessNegative(IntermediateTask<int> task)
    {
        if (task.Completed)
        {
            return task;
        }

        return task.Value < 0
            ? new IntermediateTask<int>(-1, true)
            : new IntermediateTask<int>(task.Value, false);
    }

    private static IntermediateTask<int> ProcessMod2(IntermediateTask<int> task)
    {
        if (task.Completed)
        {
            return task;
        }

        return task.Value % 2 == 0 ?
            new IntermediateTask<int>(task.Value / 2, true) :
            new IntermediateTask<int>(task.Value * 2, true);
    }

    private static int UnwrapTask(IntermediateTask<int> task)
    {
        return task.Completed ? task.Value : -1;
    }

    private static void LogError<T>(Result<T, string> result)
    {
        if (!result.HasValue)
        {
            Console.WriteLine(result.Error);
        }
    }

    private readonly struct IntermediateTask<T>(T value, bool completed)
    {
        public T Value { get; } = value;

        public bool Completed { get; } = completed;
    }
}
